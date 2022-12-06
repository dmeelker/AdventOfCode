use std::fs;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<i32> {
    input.split(',').map(|v| v.parse().unwrap()).collect()
}

fn part1(values: &[i32]) -> i32 {
    let min = *values.iter().min().unwrap();
    let max = *values.iter().max().unwrap();
    
    (min..max)
        .map(|i| values.iter().map(|v| 
            (v - i).abs()).sum())
        .min().unwrap()
}

fn part2(values: &[i32]) -> i32 {
    let min = *values.iter().min().unwrap();
    let max = *values.iter().max().unwrap();

    (min..max)
        .map(|i| values.iter().map(|v|
                triangular_number((v - i).abs()))
            .sum())
        .min().unwrap()
}

fn triangular_number(size: i32) -> i32 {
    (size * (size+1)) / 2
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let values = parse_input(&input);
    
        let part1 = part1(&values);

        assert_eq!(356958, part1);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let values = parse_input(&input);
    
        let part1 = part2(&values);

        assert_eq!(105461913, part1);
    }
}