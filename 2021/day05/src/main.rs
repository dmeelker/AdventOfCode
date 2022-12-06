use std::{fs, collections::HashMap};
use itertools::Itertools;

#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub struct Point {
    pub x: i32, 
    pub y: i32,
}

fn main() {
    let input = fs::read_to_string("input2.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<(Point, Point)> {
    input.lines().map(parse_line).collect()
}

fn parse_line(input: &str) -> (Point, Point) {
    let parts: (&str, &str) = input.split(" -> ").collect_tuple().unwrap();
    (parse_coordinate(parts.0), parse_coordinate(parts.1))
}

fn parse_coordinate(coordinate: &str) -> Point {
    let parts: (&str, &str) = coordinate.split(',').collect_tuple().unwrap();
    Point { x: parts.0.parse().unwrap(), y: parts.1.parse().unwrap() }
}

fn part1(values: &[(Point, Point)]) -> usize {
    count_duplicate_cells(&render_lines(values, false))
}

fn part2(values: &[(Point, Point)]) -> usize {
    count_duplicate_cells(&render_lines(values, true))
}

fn render_lines(lines: &[(Point, Point)], draw_diagonals: bool) -> HashMap<Point, usize> {
    lines.iter()
        .filter(|line| !is_diagonal(&line.0, &line.1) || (is_diagonal(&line.0, &line.1) && draw_diagonals))
        .map(|line| project_line(&line.0, &line.1))
        .flatten()
        .counts_by(|point| point)
}

pub fn project_line(p1: &Point, p2: &Point) -> Vec<Point> {
    let vx = (p2.x - p1.x).signum();
    let vy = (p2.y - p1.y).signum();
    let steps = (p2.x - p1.x).abs().max((p2.y - p1.y).abs());

    let mut point = p1.clone();
    let mut points = Vec::new();

    for _ in 0..steps+1 {
        points.push(point.clone());
        point = Point { x: point.x + vx, y: point.y + vy};
    }

    points
}

fn count_duplicate_cells(cells: &HashMap<Point, usize>) -> usize {
    cells.values().filter(|count| **count > 1).count()
}

fn is_diagonal(p1: &Point, p2: &Point) -> bool {
    p1.x != p2.x && p1.y != p2.y
}