use std::fs;
use itertools::Itertools;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<String> {
    input.lines().map(String::from).collect()
}

fn part1(values: &[String]) -> i32 {
    1
}

fn part2(values: &[String]) -> i32 {
    2
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = vec![String::from("123")];
        let result = part1(&input);

        assert_eq!(1, result);
    }

    #[test]
    fn part2_should_work() {
        let input = vec![String::from("123")];
        let result = part2(&input);

        assert_eq!(2, result);
    }
}