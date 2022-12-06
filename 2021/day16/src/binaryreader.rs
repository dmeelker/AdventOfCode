use std::fmt::Binary;

use itertools::Itertools;

pub struct BinaryReader {
    position: usize,
    data: Vec<bool>
}

impl BinaryReader{
    pub fn new(data: &[bool]) -> BinaryReader {
        BinaryReader {
            position: 0,
            data: data.iter().copied().collect()
        }
    }

    pub fn from_hex(input: &str) -> BinaryReader {
        BinaryReader::new(&BinaryReader::parse_hex_string(input))
    }

    fn parse_hex_string(input: &str) -> Vec<bool> {
        input.chars()
            .flat_map(|c| {
                let binary = match c {
                    '0' => "0000",
                    '1' => "0001",
                    '2' => "0010",
                    '3' => "0011",
                    '4' => "0100",
                    '5' => "0101",
                    '6' => "0110",
                    '7' => "0111",
                    '8' => "1000",
                    '9' => "1001",
                    'A' => "1010",
                    'B' => "1011",
                    'C' => "1100",
                    'D' => "1101",
                    'E' => "1110",
                    'F' => "1111",
                    _ => panic!()
                };
    
                binary.chars().map(|c| c == '1').collect_vec()
            })
            .collect()
    }

    pub fn position(&self) -> usize {
        self.position
    }

    pub fn read_u64(&mut self, bit_count: usize) -> u64 {
        let mut bits = self.read_bits(bit_count);
        let mut factor = 1;
        let mut sum = 0;

        while !bits.is_empty() {
            let bit = bits.pop().unwrap();

            if bit {
                sum += factor;
            }

            factor *= 2;
        }
        
        sum
    }

    pub fn read_bits(&mut self, quantity: usize) -> Vec<bool> {
        if self.can_read(quantity) {
            let data = self.data[self.position..self.position + quantity].iter().copied().collect();
            self.position += quantity;
            data
        } else {
            panic!();
        }
    }

    pub fn can_read(&self, quantity: usize) -> bool {
        self.data.len() - self.position >= quantity
    }
}