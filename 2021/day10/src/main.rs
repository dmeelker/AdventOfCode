use std::fs;
use itertools::Itertools;

enum Line {
    Corrupt{last_char: char},
    Incomplete{stack: Vec<char>}
}

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

fn part1(values: &[String]) -> u32 {
    analyse_lines(values).iter().map(|l| match l {
        Line::Corrupt { last_char} => get_corrupt_char_score(*last_char),
        _ => 0
    }).sum()
}

fn analyse_lines(values: &[String]) -> Vec<Line> {
    let mut result = Vec::new();

    'lineloop: for line in values {
        let mut stack = Vec::new();

        for char in line.chars() {
            if "([{<".contains(char) {
                stack.push(char);
            } else {
                let open = stack.pop().unwrap();

                if !equal_type(open, char) {
                    result.push(Line::Corrupt{ last_char: char});
                    continue 'lineloop;
                }
            }
        }

        if !stack.is_empty() {
            result.push(Line::Incomplete { stack });
        }
    }

    result
}

fn equal_type(c1: char, c2: char) -> bool {
    match (c1, c2) {
        ('(', ')') => true,
        ('{', '}') => true,
        ('[', ']') => true,
        ('<', '>') => true,
        _ => false
    }
}

fn get_corrupt_char_score(c: char) -> u32 {
    match c {
        ')' => 3,
        '}' => 57,
        ']' => 1197,
        '>' => 25137,
        _ => panic!()
    }
}

fn part2(values: &[String]) -> u64 {
    let scores = analyse_lines(values).iter()
        .filter_map(|line| {
            if let Line::Incomplete{stack} = line {
                Some(calculate_incomplete_line_score(stack))
            } else {
                None
            }
        }).collect_vec();

    *scores.iter().sorted().nth(scores.len() / 2).unwrap()
}

fn calculate_incomplete_line_score(stack: &Vec<char>) -> u64 {
    stack.iter().rev()
        .map(|c| get_missing_char_score(*c))
        .fold(0, |acc, value| (acc * 5) + value)
}

fn get_missing_char_score(c: char) -> u64 {
    match c {
        '(' => 1,
        '{' => 3,
        '[' => 2,
        '<' => 4,
        _ => panic!()
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let values = parse_input(&input);
        let result = part1(&values);

        assert_eq!(26397, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let values = parse_input(&input);
        let result = part2(&values);

        assert_eq!(288957, result);
    }
}