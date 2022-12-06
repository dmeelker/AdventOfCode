use std::fs;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<usize> {
    input.split(',').map(|p| p.parse().unwrap()).collect()
}

fn part1(values: &[usize]) -> usize {
    simulate_fish(values, 80)
}

fn part2(values: &[usize]) -> usize {
    simulate_fish(values, 256)
}

fn simulate_fish(values: &[usize], days: usize) -> usize {
    let mut fish = [0_usize; 9];

    for age in values {
        fish[*age] += 1;
    }

    (0..days).fold(fish, |fish, _| simulate_day(&fish))
        .iter().sum()
}

fn simulate_day(input: &[usize; 9]) -> [usize; 9] {
    let mut new_fish = [0_usize; 9];
    
    new_fish[..(input.len() - 1)].clone_from_slice(&input[1..]);
    new_fish[8] = input[0];
    new_fish[6] += input[0];
    new_fish
}