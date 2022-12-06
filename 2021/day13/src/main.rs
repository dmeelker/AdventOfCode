use core::panic;
use std::{fs, collections::HashSet};
use itertools::Itertools;

#[derive(Debug, Hash, Eq, PartialEq, Clone, Copy)]
struct Point {
    x: i32,
    y: i32,
}

impl Point {
    fn new(x: i32, y: i32) -> Point {
        Point {x, y}
    }
}

#[derive(Debug, Clone, Copy)]
enum Fold {
    X(i32),
    Y(i32),
}

#[derive(Debug)]
struct Input {
    points: Vec<Point>,
    folds: Vec<Fold>,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let input = parse_input(&input);

    let part1 = part1(&input);
    let part2 = part2(&input);

    println!("Part 1: {}", part1);
    println!("Part 2:\n{}", part2);
}

fn parse_input(input: &str) -> Input {
    let parts = input.split_once("\r\n\r\n").unwrap();

    Input { 
        points: parse_points(parts.0),
        folds: parse_folds(parts.1),
    }
}

fn parse_points(input: &str) -> Vec<Point> {
    input.lines().map(|line| {
        let values = line.split_once(',').unwrap();
        Point::new(
            values.0.parse().unwrap(),
            values.1.parse().unwrap()
        )
    }).collect_vec()
}

fn parse_folds(input: &str) -> Vec<Fold> {
    input.lines().map(|line| {
        let values = line[11..].split_once('=').unwrap();
        let value: i32 = values.1.parse().unwrap();
    
        match values.0 {
            "y" => Fold::Y(value),
            "x" => Fold::X(value),
            _ => panic!()
        }
    }).collect_vec()
}

fn fold(points: &HashSet<Point>, fold: Fold) -> HashSet<Point> {
    match fold {
        Fold::X(fold_x) => {
            let left = points.iter().filter(|p| p.x < fold_x).copied();
            let right = points.iter().filter(|p| p.x > fold_x).map(|p| Point::new(fold_coordinate(p.x, fold_x), p.y));
            left.chain(right).collect()
        },
        Fold::Y(fold_y) => {
            let top = points.iter().filter(|p| p.y < fold_y).copied();
            let bottom = points.iter().filter(|p| p.y > fold_y).map(|p| Point::new(p.x, fold_coordinate(p.y, fold_y)));
            top.chain(bottom).collect()
        }
    }
}

fn fold_coordinate(coordinate: i32, fold_location: i32) -> i32 {
    fold_location - (coordinate - fold_location)
}

fn part1(input: &Input) -> usize {
    let mut points:  HashSet<Point> = input.points.iter().copied().collect();
    points = fold(&points, input.folds[0]);
    points.len()
}

fn part2(input: &Input) -> String {
    let points = input.folds.iter()
        .fold(
            input.points.iter().copied().collect(),
            |points, f| fold(&points, *f)
        );
    
    render_points(&points)
}

fn render_points(points: &HashSet<Point>) -> String {
    let mut chars: Vec<char> = Vec::new();
    let max_x = points.iter().map(|p| p.x).max().unwrap();
    let max_y = points.iter().map(|p| p.y).max().unwrap();

    for y in 0..=max_y {
        for x in 0..=max_x {
            let point = points.contains(&Point::new(x as i32, y as i32));

            if point {
                chars.push('#');
            } else {
                chars.push('.');
            }
        }

        chars.push('\n');
    }

    chars.iter().collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let input = parse_input(&input);
        let result = part1(&input);

        assert_eq!(664, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let input = parse_input(&input);
        let result = part2(&input);

        assert_eq!("####.####...##.#..#.####.#....###..#...
#....#.......#.#.#.....#.#....#..#.#...
###..###.....#.##.....#..#....###..#...
#....#.......#.#.#...#...#....#..#.#...
#....#....#..#.#.#..#....#....#..#.#...
####.#.....##..#..#.####.####.###..####
", result);
    }
}
