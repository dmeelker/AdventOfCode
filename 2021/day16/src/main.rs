use core::panic;
use std::{fs};

use crate::binaryreader::BinaryReader;

mod binaryreader;

#[derive(Debug)]
enum Packet {
    Literal{version: u64, value: u64},
    Operator{version: u64, type_id: u64, operands: Vec<Packet>}
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let part1 = part1(&input);
    let part2 = part2(&input);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn part1(input: &str) -> usize {
    let mut stream = BinaryReader::from_hex(input);
    let packet = parse_packet(&mut stream);

    get_version_sum(&packet)
}

fn get_version_sum(packet: &Packet) -> usize {
    match packet {
        Packet::Literal{version, value: _} => *version as usize,
        Packet::Operator{version, type_id: _, operands} => (*version as usize) + operands.iter().map(get_version_sum).sum::<usize>(),
    }
}

fn part2(input: &str) -> usize {
    let mut stream = BinaryReader::from_hex(input);
    let packet = parse_packet(&mut stream);

    evaluate(&packet)
}

const OP_SUM: u64 = 0;
const OP_PRODUCT: u64 = 1;
const OP_MINIMUM: u64 = 2;
const OP_MAXIMUM: u64 = 3;
const OP_GT: u64 = 5;
const OP_LT: u64 = 6;
const OP_EQ: u64 = 7;

fn evaluate(packet: &Packet) -> usize {
    match packet {
        Packet::Literal {version: _, value} => *value as usize,
        Packet::Operator {version: _, type_id: OP_SUM, operands} => operands.iter().map(evaluate).sum(),
        Packet::Operator {version: _, type_id: OP_PRODUCT, operands} => operands.iter().map(evaluate).product(),
        Packet::Operator {version: _, type_id: OP_MINIMUM, operands} => operands.iter().map(evaluate).min().unwrap(),
        Packet::Operator {version: _, type_id: OP_MAXIMUM, operands} => operands.iter().map(evaluate).max().unwrap(),
        Packet::Operator {version: _, type_id: OP_GT, operands} => if evaluate(&operands[0]) > evaluate(&operands[1]) { 1 } else { 0 },
        Packet::Operator {version: _, type_id: OP_LT, operands} => if evaluate(&operands[0]) < evaluate(&operands[1]) { 1 } else { 0 },
        Packet::Operator {version: _, type_id: OP_EQ, operands} => if evaluate(&operands[0]) == evaluate(&operands[1]) { 1 } else { 0 },
        _ => panic!()
    }
}

const LITERAL_PACKET_TYPE: u64 = 4;

fn parse_packet(input: &mut BinaryReader) -> Packet {
    let version = input.read_u64(3);
    let type_id = input.read_u64(3);

    if type_id == LITERAL_PACKET_TYPE {
        parse_literal_packet(input, version)
    } else {
        parse_operator_packet(input, version, type_id)
    }
}

fn parse_literal_packet(input: &mut BinaryReader, version: u64) -> Packet {
    let mut data = Vec::new();
    loop {
        let last_block = input.read_u64(1);
        let block_data = input.read_bits(4);

        for b in block_data.iter() {
            data.push(*b);
        }

        if last_block == 0 {
            break;
        }
    }
    Packet::Literal{version, value: BinaryReader::new(&data).read_u64(data.len())}
}

fn parse_operator_packet(input: &mut BinaryReader, version: u64, type_id: u64) -> Packet {
    let length_type = input.read_u64(1);
    match length_type {
        0 => {
            let bit_count = input.read_u64(15);
            let end_position = input.position() + bit_count as usize;
            let mut operands = Vec::new();

            loop {
                operands.push(parse_packet(input));

                if input.position() == end_position {
                    break;
                } else if input.position() > end_position {
                    panic!();
                }
            }

            Packet::Operator {version, type_id, operands }
        },
        1 => {
            let packet_count = input.read_u64(11);
            let mut operands = Vec::new();

            for _ in 0..packet_count {
                operands.push(parse_packet(input));
            }

            Packet::Operator {version, type_id, operands }
        }
        _ =>panic!()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input =  fs::read_to_string("input.txt").unwrap();
        let result = part1(&input);

        assert_eq!(897, result);
    }

    #[test]
    fn part2_should_work() {
        let input =  fs::read_to_string("input.txt").unwrap();
        let result = part2(&input);

        assert_eq!(9485076995911, result);
    }
}