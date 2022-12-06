use std::{fs, collections::HashSet};
use itertools::Itertools;

#[derive(Debug, Eq, PartialEq, Hash, Clone, Copy)]
struct Point {
    x: i32,
    y: i32
}

impl Point {
    fn new(x: i32, y: i32) -> Point {
        Point {x,y}
    }
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values, 100);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<Vec<i32>> {
    input.lines().map(|line| 
        line.chars().map(|v| v.to_string().parse::<i32>().unwrap()).collect_vec()
    ).collect()
}

fn part1(values: &[Vec<i32>], steps: i32) -> usize {
    let mut values = values.iter().cloned().collect_vec();
    let mut flashes = 0;

    for _ in 0..steps {
        flashes += simulate_step(&mut values);
    }
    
    flashes
}

fn part2(values: &[Vec<i32>]) -> i32 {
    let mut values = values.iter().cloned().collect_vec();
    let mut step = 0;

    loop {
        step += 1;
        let flashes = simulate_step(&mut values);
        if all_cells_flashed(flashes, &values) {
            break;
        }
    }

    step
}

fn all_cells_flashed(flashes: usize, values: &[Vec<i32>]) -> bool {
    flashes == values.len() * values[0].len()
}

fn simulate_step(values: &mut Vec<Vec<i32>>) -> usize {
    increase_energy(values);
    let flashes = process_flashes(values);

    flashes.len()
}

fn increase_energy(values: &mut Vec<Vec<i32>>) {
    for cell in all_cells(values[0].len(), values.len()) {
        values[cell.y as usize][cell.x as usize] += 1;
    }
}

fn process_flashes(values: &mut Vec<Vec<i32>>) -> HashSet<Point>{
    let mut flashed = HashSet::new();

    loop {
        let mut any_flash = false;
        
        for cell in all_cells(values[0].len(), values.len()) {
            if get(&cell, values) > 9 && !flashed.contains(&cell) {
                flashed.insert(cell);
                flash(&cell, values);
                any_flash = true;
            }
        }

        if !any_flash {
            break;
        }
    }

    for cell in flashed.iter() {
        values[cell.y as usize][cell.x as usize] = 0;
    }

    flashed
}

fn flash(location: &Point, values: &mut Vec<Vec<i32>>) {
    for neighbour in get_neighbours(*location) {
        increase(&neighbour, values);    
    }
}

fn all_cells<'a>(width: usize, height: usize) -> impl Iterator<Item = Point> + 'a {
    (0..width).flat_map(move |x| 
        (0..height).map(move |y| 
            Point::new(x as i32, y as i32)
        )
    )
}

fn get_neighbours(location: Point) -> impl Iterator<Item = Point> {
    (-1..=1).flat_map(move |x| 
        (-1..=1).map(move |y| 
            Point::new(location.x + x, location.y + y)
        )
    ).filter(move |l| *l != location)
}

fn increase(location: &Point, values: &mut Vec<Vec<i32>>) {
    if !in_range(location, values) {
        return;
    }

    values[location.y as usize][location.x as usize] += 1;
}

fn get(location: &Point, values: &[Vec<i32>]) -> i32{
    if !in_range(location, values) {
        panic!();
    }

    values[location.y as usize][location.x as usize]
}

fn in_range(location: &Point, values: &[Vec<i32>]) -> bool {
    !(location.x < 0 || location.x >= values[0].len() as i32 || location.y < 0 || location.y >= values.len() as i32)
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let values = parse_input(&input);
        let result = part1(&values, 100);

        assert_eq!(1615, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let values = parse_input(&input);
        let result = part2(&values);

        assert_eq!(249, result);
    }
}