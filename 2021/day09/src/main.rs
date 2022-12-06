use std::{fs, collections::HashSet};
use itertools::Itertools;

#[derive(Debug, Eq, PartialEq, Hash, Clone)]
struct Point {
    x: i32,
    y: i32,
}

impl Point {
    fn new(x: i32, y: i32) -> Point {
        Point {x, y}
    }
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<Vec<i32>> {
    input.lines().map(|line| line.trim().chars().map(|c| c.to_string().parse().unwrap()).collect_vec()).collect()
}

fn part1(values: &[Vec<i32>]) -> i32 {
    find_low_points(values).iter().map(|p| values[p.y as usize][p.x as usize] + 1).sum()
}

fn part2(values: &[Vec<i32>]) -> i32 {
    find_low_points(values).iter()
        .map(|p| get_basin_size(p, values))
        .sorted()
        .rev()
        .take(3)
        .product()
}

fn find_low_points(values: &[Vec<i32>]) -> Vec<Point> {
    let mut result = Vec::new();

    for cell in all_grid_cells(values[0].len(), values.len()) {
        let value = get_cell(&cell, values);
        let neighbours = get_neighbours(&cell, values);

        if neighbours.iter().all(|neighbour| *neighbour > value) {
            result.push(cell.clone());
        }
    }

    result
}

fn all_grid_cells(width: usize, height: usize) -> Vec<Point> {
    let mut points = Vec::new();
    for y in 0..height {
        for x in 0..width {
            points.push(Point::new(x as i32, y as i32));
        }
    }
    points
}

fn get_neighbours(point: &Point, values: &[Vec<i32>]) -> Vec<i32> {
    get_neighbour_cells(point).iter().map(|p| get_cell(p, values)).collect_vec()
}

fn get_neighbour_cells(p: &Point) -> Vec<Point> {
    vec![
        Point {x: p.x, y: p.y - 1},
        Point {x: p.x, y: p.y + 1},
        Point {x: p.x - 1, y: p.y},
        Point {x: p.x + 1, y: p.y}]
}

fn get_cell(point: &Point, values: &[Vec<i32>]) -> i32 {
    if point.x >= 0 && point.x < values[0].len() as i32 && point.y >= 0 && point.y < values.len() as i32 {
        values[point.y as usize][point.x as usize]
    } else {
        i32::MAX
    }
}

fn get_basin_size(point: &Point, values: &[Vec<i32>]) -> i32 {
    let mut open = vec![point.clone()];
    let mut closed: HashSet<Point> = HashSet::new();
    let mut count = 0;

    while !open.is_empty() {
        let point = open.pop().unwrap();
        closed.insert(point.clone());
        count += 1;

        for neighbour in get_neighbour_cells(&point).iter() {
            if !closed.contains(neighbour) && !open.contains(neighbour) && get_cell(neighbour, values) < 9 {
                open.push(neighbour.clone());
            }
        }
    }

    count
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let values = parse_input(&input);
        let result = part1(&values);

        assert_eq!(15, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let values = parse_input(&input);
        let result = part2(&values);

        assert_eq!(1134, result);
    }
}