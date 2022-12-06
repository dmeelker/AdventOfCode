use std::fs;
use itertools::Itertools;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<i32> {
    input.lines().map(|l| l.parse().unwrap()).collect()
}

fn part1(values: &[i32]) -> i32 {
    let mut last_value = None;
    let mut increase_count = 0;

    for value in values {
        if let Some(last_value) = last_value {
            if value > last_value {
                increase_count += 1;
            }
        }

        last_value = Some(value);
    }

    increase_count
}

fn part2(values: &[i32]) -> i32 {
    let values = group_values(values);
    part1(&values)
}

fn group_values(values: &[i32]) -> Vec<i32> {
    values.iter().tuple_windows::<(_, _, _)>()
        .map(|g| g.0 + g.1 + g.2)
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = vec![2,1,3,4];
        let result = part1(&input);

        assert_eq!(2, result);
    }

    #[test]
    fn group_values_should_work_for_one_group() {
        let input = vec![1,2,3];
        let result = group_values(&input);

        assert_eq!(1, result.len());
        assert_eq!(6, result[0]);
    }

    #[test]
    fn group_values_should_work_for_two_groups() {
        let input = vec![1,2,3,4];
        let result = group_values(&input);

        assert_eq!(2, result.len());
        assert_eq!(6, result[0]);
        assert_eq!(9, result[1]);
    }
}