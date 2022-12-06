use std::{fs, vec, collections::HashSet};
use itertools::Itertools;

#[derive(Debug, Clone, Copy)]
struct TargetArea {
    min_x: i32,
    max_x: i32,
    min_y: i32,
    max_y: i32,
}

#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
struct Point {
    x: i32,
    y: i32,
}

struct Shot {
    vector: Point,
    max_y: i32,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> TargetArea {
    let parts = input[13..].split_once(",").unwrap();
    let x_range = parse_range(parts.0.trim());
    let y_range = parse_range(parts.1.trim());

    TargetArea {
        min_x: x_range.0,
        max_x: x_range.1,
        min_y: y_range.0,
        max_y: y_range.1,
    }
}

fn parse_range(input: &str) -> (i32, i32) {
    let parts = input[2..].split_once("..").unwrap();

    (
        parts.0.parse().unwrap(),
        parts.1.parse().unwrap(),
    )
}

fn part1(target: &TargetArea) -> i32 {
    find_hits(target).iter().map(|hit| hit.max_y).max().unwrap()
}

fn part2(target: &TargetArea) -> usize {
    let hits: HashSet<Point> = find_hits(target).iter().map(|hit| hit.vector).collect();
    hits.len()
}

fn find_hits(target: &TargetArea) -> Vec<Shot> {
    let mut hits = Vec::new();

    for x in 0..500 {
        for y in -500..500 {
            let path = plot_path(&Point {x: 0, y: 0}, &Point {x, y}, &target);

            if path_intersects_target(&path, target) {
                hits.push(Shot {
                    vector: Point {x, y},
                    max_y: path.iter().map(|point| point.y).max().unwrap()
                });
            }
        }
    }

    hits
}

fn plot_path(start: &Point, vector: &Point, target: &TargetArea) -> Vec<Point> {
    let mut path = Vec::new();
    let mut vector = *vector;
    let mut location = *start;

    path.push(*start);

    loop {
        location.x += vector.x;
        location.y += vector.y;
        path.push(location.clone());

        if location.y < target.min_y || location.x > target.max_x {
            break;
        }

        vector.x = (vector.x - 1).max(0);
        vector.y -= 1;
    }

    path
}

fn path_intersects_target(path: &[Point], target: &TargetArea) -> bool {
    path.iter().any(|p| 
        p.x >= target.min_x &&
        p.x <= target.max_x &&
        p.y >= target.min_y &&
        p.y <= target.max_y
    )
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let target = parse_input(&input);
        let result = part1(&target);

        assert_eq!(5995, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let target = parse_input(&input);
        let result = part2(&target);

        assert_eq!(3202, result);
    }
}