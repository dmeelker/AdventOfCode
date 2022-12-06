use std::{fs, collections::HashMap, hash::Hash};
use itertools::{Itertools, MinMaxResult};

type PuzzleInput = (Vec<char>, HashMap<(char, char), char>);

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> PuzzleInput {
    let parts = input.split_once("\r\n\r\n").unwrap();
    (
        parts.0.chars().collect(),
        parse_rules(parts.1),
    )
}

fn parse_rules(input: &str) -> HashMap<(char, char), char> {
    input.lines().map(|line| {
        let parts = line.split_once(" -> ").unwrap();
        ((parts.0.chars().nth(0).unwrap(), parts.0.chars().nth(1).unwrap()), parts.1.chars().next().unwrap())
    }).collect()
}

fn part1(input: &PuzzleInput) -> usize {
    solve(input, 10)
}

fn part2(input: &PuzzleInput) -> usize {
    solve(input, 40)
}

fn solve(input: &PuzzleInput, steps: usize) ->usize {
    let mut pairs = get_pairs(&input.0);

    for _ in 0..steps
    {
        pairs = step(&pairs, &input.1);
    }

    let char_counts = get_character_counts(&input.0, &pairs);

    match char_counts.values().minmax() {
        MinMaxResult::MinMax(min, max) => max - min,
        _ => panic!()
    }
}

fn get_pairs(input: &[char]) -> HashMap<(char, char), usize> {
    let mut counts: HashMap<(char,char), usize> = HashMap::new();

    for i in 0..input.len() - 1 {
        counts.entry((input[i], input[i+1]))
            .and_modify(|count| *count += 1)
            .or_insert(1);
    }

    counts
}

fn step(pairs: &HashMap<(char,char), usize>, rules: &HashMap<(char, char), char>) -> HashMap<(char,char), usize> {
    let mut new_counts = HashMap::new();
        
    for e in pairs.iter() {
        let mapping = rules.get(e.0);
        
        if let Some(mapping) = mapping {
            add_pairs((e.0.0, *mapping), *e.1, &mut new_counts);
            add_pairs((*mapping, e.0.1), *e.1, &mut new_counts);
        } else {
            add_pairs(*e.0, *e.1, &mut new_counts);
        }
    }

    new_counts
}

fn add_pairs(pair: (char,char), amount: usize, pairs: &mut HashMap<(char,char), usize>) {
    pairs.entry(pair)
        .and_modify(|count| *count += amount)
        .or_insert(amount);
}

fn get_character_counts(elements: &[char], pairs: &HashMap<(char,char), usize>) -> HashMap<char, usize> {
    let mut counts: HashMap<char, usize> = pairs.iter().flat_map(|e| vec![(e.0.0, 0), (e.0.1, 0)]).collect();

    for pair in pairs.iter() {
        counts.entry(pair.0.0).and_modify(|count| *count += pair.1);
        counts.entry(pair.0.1).and_modify(|count| *count += pair.1);
    }

    counts.entry(*elements.first().unwrap()).and_modify(|count| *count += 1);
    counts.entry(*elements.last().unwrap()).and_modify(|count| *count += 1);

    counts.iter().map(|e| (*e.0, *e.1 / 2)).collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work_for_input() {
        let input = fs::read_to_string("input.txt").unwrap();
        let input = parse_input(&input);
        let result = part1(&input);

        assert_eq!(2233, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let input = parse_input(&input);
        let result = part2(&input);

        assert_eq!(2884513602164, result);
    }
}