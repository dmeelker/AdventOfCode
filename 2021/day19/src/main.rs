use std::fs;
use itertools::Itertools;

#[derive(Debug, Clone, PartialEq, Eq)]
struct Scanner {
    id: i32,
    beacons: Vec<Point>
}

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
struct Point {
    x: i32,
    y: i32,
    z: i32,
}

impl Point {
    fn new(x: i32, y: i32, z: i32) -> Point {
        Point {x, y, z}
    }
}

fn main() {
    let input = fs::read_to_string("input2.txt").unwrap();
    let values = parse_input(&input);

    let p = get_rotation_permutations(&values[0].beacons);

    eprintln!("p = {:?}", p);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<Scanner> {
    input.split("\r\n\r\n").map(parse_beacon).collect()
}

fn parse_beacon(input: &str) -> Scanner {
    let lines = input.lines().collect_vec();

    Scanner {
        id: parse_scanner_id(lines[0]),
        beacons: lines.iter().skip(1).map(|line| parse_location(*line)).collect(),
    }
}

fn parse_scanner_id(input: &str) -> i32 {
    input[12..input.len() - 4].parse().unwrap()
}

fn parse_location(input: &str) -> Point {
    let parts = input.split(',').collect_vec();

    Point { 
        x: parts[0].parse().unwrap(), 
        y: parts[1].parse().unwrap(),
        z: parts[2].parse().unwrap()
    }
}

fn part1(values: &[Scanner]) -> i32 {
    1
}

fn part2(values: &[Scanner]) -> i32 {
    2
}

fn get_rotation_permutations(points: &[Point]) -> Vec<Vec<Point>> {
    let mut permutations = Vec::new();

    permutations.push(mutate_point(points, |p| Point::new(p.x, p.y, p.z)));

    permutations
}

fn mutate_point(points: &[Point], func: fn(&Point) -> Point) -> Vec<Point> {
    points.iter().map(|p| func(p)).collect()
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