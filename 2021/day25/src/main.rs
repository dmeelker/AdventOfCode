use itertools::Itertools;
use std::fs;

#[derive(Clone, PartialEq, Eq)]
struct Grid {
    data: Vec<Vec<char>>,
    width: usize,
    height: usize,
}

impl Grid {
    fn new(width: usize, height: usize) -> Grid {
        let data = vec![vec!['.'; width]; height];
        Grid {
            data,
            width,
            height,
        }
    }

    fn get(&self, x: usize, y: usize) -> char {
        self.data[y][x]
    }

    fn set(&mut self, x: usize, y: usize, value: char) {
        self.data[y][x] = value
    }

    fn print(&self) {
        for y in 0..self.height {
            for x in 0..self.width {
                print!("{}", self.data[y][x]);
            }

            println!();
        }
    }
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);

    println!("Part 1: {}", part1);
}

fn parse_input(input: &str) -> Grid {
    let data: Vec<Vec<char>> = input.lines().map(|l| l.chars().collect_vec()).collect();
    let width = data[0].len();
    let height = data.len();
    Grid {
        data,
        width,
        height,
    }
}

fn part1(values: &Grid) -> i32 {
    values.print();
    let mut grid = values.clone();
    let mut iteration = 0;

    loop {
        let new_state = simulate_step(&grid);
        iteration += 1;
        if new_state == grid {
            println!();
            grid.print();
            return iteration;
        }
        grid = new_state;
    }
}

fn simulate_step(input: &Grid) -> Grid {
    let mut output = Grid::new(input.width, input.height);

    for y in 0..input.height {
        for x in 0..input.width {
            if input.get(x, y) == '>' {
                let target_x = if x + 1 == input.width { 0 } else { x + 1 };

                if input.get(target_x, y) == '.' {
                    output.set(target_x, y, '>')
                } else {
                    output.set(x, y, '>')
                }
            }
        }
    }

    for y in 0..input.height {
        for x in 0..input.width {
            if input.get(x, y) == 'v' {
                let target_y = if y + 1 == input.height { 0 } else { y + 1 };

                if input.get(x, target_y) != 'v' && output.get(x, target_y) == '.' {
                    output.set(x, target_y, 'v')
                } else {
                    output.set(x, y, 'v')
                }
            }
        }
    }

    output
}
