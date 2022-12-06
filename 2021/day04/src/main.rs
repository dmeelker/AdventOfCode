use std::fs;
use board::Board;
use itertools::Itertools;

mod board;

struct Input {
    values: Vec<i32>,
    boards: Vec<Board>,
}

fn main() {
    let input = fs::read_to_string("input2.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Input {
    let parts = input.split("\r\n\r\n").collect_vec();

    Input { 
        values: parts[0].split(',').map(|v| v.parse().unwrap()).collect_vec(), 
        boards: parts[1..].iter().map(|part| parse_board(*part)).collect()
     }
}

fn parse_board(input: &str) -> Board {
    let lines = input.lines().collect_vec();
    let values: Vec<i32> = input.lines().map(parse_board_line).flatten().collect();

    Board::new(parse_board_line(lines[0]).len(), lines.len(), &values)
}

fn parse_board_line(line: &str) -> Vec<i32> {
    line.replace("  ", " ").trim().split(' ').map(|v| v.parse().unwrap()).collect_vec()
}

fn part1(input: &Input) -> i32 {
    compute_win_scores(input)[0]
}

fn part2(input: &Input) -> i32 {
    *compute_win_scores(input).iter().last().unwrap()
}

fn compute_win_scores(input: &Input) -> Vec<i32> {
    let mut boards = input.boards.clone();
    let mut wins= vec![];

    for value in input.values.iter() {
        for board in boards.iter_mut() {
            if !board.winner() && board.mark(*value) && board.winner() {
                wins.push(board.sum_unmarked() * value);
            }
        }
    }

    wins
}