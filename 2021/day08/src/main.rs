use std::{fs, collections::HashMap};
use itertools::Itertools;

struct Entry {
    signals: Vec<String>,
    output: Vec<String>
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<Entry> {
    input.lines().map(parse_line).collect()
}

fn parse_line(line: &str) -> Entry {
    let parts = line.split('|').collect_vec();

    Entry { 
        signals: parts[0].trim().split(' ').map(String::from).collect_vec(), 
        output: parts[1].trim().split(' ').map(String::from).collect_vec(), 
    }
}

fn part1(entries: &[Entry]) -> usize {
    entries.iter().map(|entry| {
        entry.output.iter().filter(|output| vec![2, 4, 3, 7].contains(&(output.len() as i32))).count()
    }).sum()
}

fn part2(values: &[Entry]) -> i32 {
    let permutations = generate_permutations();
    let mut sum = 0;
    for entry in values {
        let signal_mapping = brute_force_signal_mapping(&entry.signals, &permutations);
        let digits: String = entry.output.iter().map(|signal| decode_signal(signal, &signal_mapping).unwrap()).collect();
        let number: i32 = digits.parse().unwrap();

        sum += number;
    }

    sum
}

fn decode_signal(signal: &str, mapping: &HashMap<char, char>) -> Result<char, String> {
    let segments: String = signal.chars().map(|c| mapping[&c]).sorted().collect();

    decode_digit(segments.as_str())
}

fn decode_digit(segments: &str) -> Result<char, String> {
    match segments {
        "abcefg" => Ok('0'),
        "cf" => Ok('1'),
        "acdeg" => Ok('2'),
        "acdfg" => Ok('3'),
        "bcdf" => Ok('4'),
        "abdfg" => Ok('5'),
        "abdefg" => Ok('6'),
        "acf" => Ok('7'),
        "abcdefg" => Ok('8'),
        "abcdfg" => Ok('9'),
        _ => Err(format!("Unexpected combination {}", segments))
    }
}

fn brute_force_signal_mapping(signals: &[String], permutations: &[HashMap<char, char>]) -> HashMap<char, char> {
    permutations.iter().filter(|p| permutation_valid(signals, p))
        .cloned().next().unwrap()
}

fn permutation_valid(signals: &[String], permutation: &HashMap<char, char>) -> bool {
    signals.iter().all(|signal| decode_signal(signal, permutation).is_ok())
}

fn generate_permutations() -> Vec<HashMap<char, char>> {
    let options = vec!['a', 'b', 'c', 'd', 'e', 'f', 'g'];

    options.iter().permutations(7).map(|p| {
        p.iter().enumerate().map(|e| (**e.1, options[e.0])).collect()
    }).collect_vec()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let values = parse_input(&input);
        let result = part1(&values);

        assert_eq!(26, result);
    }

    #[test]
    fn part2_should_work() {
        let values = parse_input("acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf");
        let result = part2(&values);

        assert_eq!(5353, result);
    }

    #[test]
    fn decode_digit_should_decode_0() {
        test_decode_digit(
            "
             a 
            b c
            e f
             g", '0');
    }

    #[test]
    fn decode_digit_should_decode_1() {
        test_decode_digit("cf", '1');
    }

    #[test]
    fn decode_digit_should_decode_2() {
        test_decode_digit(
            "
         a 
          c
         d 
        e  
         g", '2');
    }

    #[test]
    fn decode_digit_should_decode_3() {
        test_decode_digit(
            " 
             a 
              c
             d 
              f
             g", '3');
    }

    #[test]
    fn decode_digit_should_decode_4() {
        test_decode_digit(
            "  
            b c
             d 
              f", '4');
    }

    #[test]
    fn decode_digit_should_decode_5() {
        test_decode_digit(
            "
             a 
            b 
             d 
              f
             g", '5');
    }

    #[test]
    fn decode_digit_should_decode_6() {
        test_decode_digit(
            "
             a 
            b
             d 
            e f
             g", '6');
    }

    #[test]
    fn decode_digit_should_decode_7() {
        test_decode_digit(
            "
            a 
             c
             f", '7');
    }

    #[test]
    fn decode_digit_should_decode_8() {
        test_decode_digit(
            " 
             a 
            b c
             d 
            e f
             g", '8');
    }

    #[test]
    fn decode_digit_should_decode_9() {
        test_decode_digit(
            "
             a 
            b c
             d 
            f
             g", '9');
    }

    fn test_decode_digit(input: &str, expected: char) {
        let result = decode_digit(to_signal_string(input).as_str());
        assert_eq!(expected, result.unwrap());
    }

    fn to_signal_string(input: &str) -> String {
        let normalized = input
            .replace(' ', "")
            .replace('\r', "")
            .replace('\n', "");

        normalized.chars().sorted().collect()
    }
}