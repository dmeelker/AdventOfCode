use std::fs;
use grid::Grid;
use itertools::Itertools;

mod grid;

#[derive(Debug)]
struct Input {
    lookup: Vec<bool>,
    image: Grid,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);
    
    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Input {
    let parts = input.split_once("\r\n\r\n").unwrap();

    Input {
        lookup: parts.0.chars().map(|c| c == '#').collect_vec(),
        image: Grid { data: parts.1.lines().map(|l| l.chars().map(|c| c == '#').collect_vec()).collect_vec() },
    }
}

fn part1(input: &Input) -> usize {
    let image = simulate(&input.image, &input.lookup, 2);
    image.count_lit()
}

fn part2(input: &Input) -> usize {
    let image = simulate(&input.image, &input.lookup, 50);
    image.count_lit()
}

fn simulate(image: &Grid, lookups: &[bool], steps: i32) -> Grid {
    let mut grid = image.clone();
    
    for i in 0..steps {
        grid = simulate_step(&grid, lookups, i);
    }

    grid
}

fn simulate_step(image: &Grid, lookups: &[bool], step: i32) -> Grid {
    let default = if step == 0 { false } else {image.get(0, 0)};
    let image = image.grow(3, default);
    
    let mut output = image.clone();

    for y in 1..image.height()-1 {
        for x in 1..image.width()-1 {
            let values = get_neighbours(&image, x, y);
            let value = get_lookup_value(&values);
            let new_value = lookups[value as usize];

            output.set(x, y, new_value);
        }    
    }

    output.shrink()
}

fn get_neighbours(image: &Grid, x: usize, y: usize) -> Vec<bool> {
    (0..3).cartesian_product(0..3).map(|p| 
        image.get(
            (x as i32 + (p.1 - 1)) as usize, 
            (y as i32 + (p.0 - 1)) as usize
        )
    ).collect()
}

fn get_lookup_value(binary: &[bool]) -> usize {
    let mut sum = 0;
    let mut factor = 1;

    for value in binary.iter().rev() {
        if *value {
            sum += factor;
        }

        factor *= 2;
    }

    sum
}

fn print_image(image: &Grid) {
    for y in 0..image.height() {
        for x in 0..image.width() {
            print!("{}", if image.get(x, y) { "#" } else {"."});
        }    
        println!();
    }
    println!();
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = parse_input(&fs::read_to_string("input.txt").unwrap());
        let result = part1(&input);

        assert_eq!(5097, result);
    }

    #[test]
    fn part2_should_work() {
        let input = parse_input(&fs::read_to_string("input.txt").unwrap());
        let result = part2(&input);

        assert_eq!(17987, result);
    }
}