use std::{fs, collections::{HashSet, HashMap}};
use itertools::Itertools;
use priority_queue::PriorityQueue;

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct Point {
    x: i32,
    y: i32,
}

impl Point {
    fn new(x: i32, y: i32) -> Point {
        Point {x, y}
    }
}

#[derive(Debug)]
struct Grid {
    width: usize,
    height: usize,
    data: Vec<Vec<i32>>
}

impl Grid {
    fn new(width: usize, height: usize, default: i32) -> Grid {
        let mut data = Vec::new();

        for _ in 0..height {
            let mut column = Vec::new();
            column.resize(width, default);
            data.push(column);
        }

        Grid { width, height, data }
    }

    fn from_data(data: Vec<Vec<i32>>) -> Grid {
        let width = data[0].len();
        let height = data.len();

        Grid {
            width,
            height,
            data
        }
    }

    fn contains(&self, point: &Point) -> bool {
        !(point.x < 0 || point.x >= self.width as i32 || point.y < 0 || point.y >= self.height as i32)
    }

    fn set(&mut self, point: &Point, value: i32) {
        if self.contains(point) {
            self.data[point.y as usize][point.x as usize] = value;
        } else {
            panic!();
        }
    }

    fn get(&self, point: &Point) -> Option<i32> {
        if self.contains(point) {
            Some(self.data[point.y as usize][point.x as usize])
        } else {
            None
        }
    }

    fn get_all_points(&self) -> Vec<Point> {
        let mut points = Vec::new();

        for y in 0..self.height {
            for x in 0..self.width {
                points.push(Point {x: x as i32, y: y as i32});
            }    
        }

        points
    }

    fn print(&self) {
        for y in 0..self.height {
            for x in 0..self.width {
                print!("{}", self.data[y][x])
            }    

            println!();
        }
    }
}

fn main() {
    let input = fs::read_to_string("input2.txt").unwrap();
    let grid = parse_input(&input);
    grid.print();
    let part1 = part1(&grid);
    let part2 = part2(&grid);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Grid {
    Grid::from_data(input.lines().map(|line|
        line.chars().map(|c| c.to_string().parse().unwrap()).collect()
    ).collect())
}

fn part1(grid: &Grid) -> i32 {
    let path = find_best_path(grid, &Point {x: 0, y: 0} , &Point{ x: (grid.width - 1) as i32, y: (grid.height - 1) as i32});

    path.iter().skip(1).map(|p| grid.get(p).unwrap()).sum()
}

fn part2(grid: &Grid) -> i32 {
    let grid = prepare_part2_grid(grid);
    let path = find_best_path(&grid, &Point {x: 0, y: 0} , &Point{ x: (grid.width - 1) as i32, y: (grid.height - 1) as i32});

    path.iter().skip(1).map(|p| grid.get(p).unwrap()).sum()
}

fn prepare_part2_grid(input: &Grid) -> Grid {
    let mut grid = Grid::new(input.width*5, input.height*5, 0);

    for y in 0..grid.height {
        for x in 0..grid.width {
            let mut value = input.get(&Point::new((x % input.width) as i32, (y % input.height) as i32)).unwrap() - 1;
            let increment = ((x / input.width) + (y / input.height)) as i32;
            value = ((value + increment) % 9) + 1;

            grid.set(&Point::new(x as i32, y as i32), value);
        }
    }

    grid
}

#[derive(Debug, PartialEq, Eq, Hash, Clone)]
struct Node {
    location: Point,
    cost: i32,
    path_cost: i32,
    path_source: Option<Point>
}

impl Node {
    fn from_point(point: &Point, cost: i32, is_start: bool) -> Node {
        Node 
        {
            location: *point,
            cost,
            path_cost: if is_start { 0 } else {i32::MAX},
            path_source: None
        }
    }
}

fn find_best_path(grid: &Grid, start: &Point, end: &Point) -> Vec<Point> {
    let mut nodes: HashMap<Point, Node> = grid.get_all_points().iter().map(|point| 
        (
            *point,
            Node::from_point(point, grid.get(point).unwrap(), point == start)
        )).collect();

    let mut unvisited_set: HashSet<Point> = nodes.keys().cloned().collect();
    let mut unvisited_queue: PriorityQueue<Point, i32> = PriorityQueue::new();
    
    for p in unvisited_set.iter() {
        unvisited_queue.push(*p,  0);
    }

    let mut current_location = *start;

    while current_location != *end {
        let neighbours = get_neighbours(&current_location);
        let current_node = nodes.get(&current_location).unwrap().clone();

        for neighbour in neighbours.iter() {
            if !unvisited_set.contains(neighbour) {
                continue;
            }

            let neighbour_node = nodes.get_mut(*&neighbour).unwrap();
            let cost_via_current_node = current_node.path_cost + neighbour_node.cost;
            
            if cost_via_current_node < neighbour_node.path_cost {
                neighbour_node.path_cost = cost_via_current_node;
                neighbour_node.path_source = Some(current_location);
                unvisited_queue.push_increase(*neighbour, i32::MAX - neighbour_node.path_cost);
            }
        }

        unvisited_set.remove(&current_location);
        current_location = unvisited_queue.pop().unwrap().0;
    }

    get_path(nodes, end, start).iter().map(|p| *p).rev().collect()
}

fn get_path(nodes: HashMap<Point, Node>, end: &Point, start: &Point) -> Vec<Point> {
    let mut path = Vec::new();
    let mut node = nodes.get(end).unwrap();

    while node.path_source.is_some() {
        path.push(node.location);
        node = match node.path_source {
            Some(previous) => nodes.get(&previous).unwrap(),
            _ => panic!()
        }
    }
    path.push(*start);

    path
}

fn get_neighbours(center: &Point) -> Vec<Point> {
    vec![
        Point { x: center.x - 0, y: center.y - 1},
        Point { x: center.x - 1, y: center.y},
        Point { x: center.x + 1, y: center.y},
        Point { x: center.x - 0, y: center.y + 1},
    ]
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let input = parse_input(&input);
        let result = part1(&input);

        assert_eq!(40, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let input = parse_input(&input);
        let result = part2(&input);

        assert_eq!(315, result);
    }
}