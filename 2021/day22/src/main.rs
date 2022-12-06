use std::fs;
use itertools::Itertools;

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct Point {
    x: i32,
    y: i32,
    z: i32
}

impl Point {
    fn new(x: i32, y: i32, z: i32) -> Point {
        Point {x, y, z}
    }
}

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct Rect{
    x1: i32,
    y1: i32,
    x2: i32,
    y2: i32,
}


impl Rect {
    fn new(x1: i32, y1: i32, x2: i32, y2: i32) -> Rect {
        Rect {x1, y1, x2, y2}
    }
}

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct Cuboid {
    p1: Point,
    p2: Point,
    area: u64,
}

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct CuboidInstruction {
    on: bool,
    cuboid: Cuboid,
}

impl Cuboid {
    fn new(p1: Point, p2: Point) -> Cuboid {
        Cuboid { p1, p2, area: (p2.x - p1.x) as u64 * (p2.y - p1.y) as u64 * (p2.z - p1.z) as u64}
    }

    fn overlaps(&self, other: &Cuboid) -> bool {
        !(
            self.p1.x > other.p2.x ||
            self.p2.x < other.p1.x ||
            self.p1.y > other.p2.y ||
            self.p2.y < other.p1.y ||
            self.p1.z > other.p2.z ||
            self.p2.z < other.p1.z
        )
    }

    fn intersect(&self, other: &Cuboid) -> Option<Cuboid> {
        if !self.overlaps(other) {
            return None;
        }

        let x1 = self.p1.x.max(other.p1.x);
        let x2 = self.p2.x.min(other.p2.x);
        let y1 = self.p1.y.max(other.p1.y);
        let y2 = self.p2.y.min(other.p2.y);
        let z1 = self.p1.z.max(other.p1.z);
        let z2 = self.p2.z.min(other.p2.z);

        Some(Cuboid::new(
            Point {x: x1, y: y1, z: z1},
            Point {x: x2, y: y2, z: z2}))
    }

    fn subtract(&self, other: &Cuboid) -> Vec<Cuboid> {
        let intersection = self.intersect(other);

        if let None = intersection {
            return vec![self.clone()];
        }

        let intersection = intersection.unwrap();
        let top_rects = self.subdivide_plane(&intersection, |p| p.x, |p| p.z);
        let front_rects = self.subdivide_plane(&intersection, |p| p.x, |p| p.y);
        let side_rects = self.subdivide_plane(&intersection, |p| p.z, |p| p.y);

        let mut cuboids = Vec::new();

        for x in 0..3 {
            for y in 0..3 {
                for z in 0..3 {
                    if x == 1 && y == 1 && z == 1 {
                        // Skip intersection
                        continue;
                    }
                    cuboids.push(Cuboid::from_rects(&top_rects[x + (z * 3)], &front_rects[x + (y * 3)], &side_rects[z + (y * 3)]));
                }
            }   
        }
        
        cuboids.iter().copied().filter(|c| c.is_3d()).collect()
    }

    fn from_rects(top: &Rect, front: &Rect, side: &Rect) -> Cuboid {
        Cuboid::new(
            Point::new(top.x1, front.y1, side.x1),
            Point::new(top.x2, front.y2, side.x2)
        )
    }

    fn subdivide_plane(&self, intersection: &Cuboid, dimh: fn(&Point) -> i32, dimv: fn(&Point) -> i32) -> Vec<Rect> {
        let hdivisions = self.subdivide_dimension(intersection, dimh);
        let vdivisions = self.subdivide_dimension(intersection, dimv);

        let mut result = Vec::new();
        
        for x in 0..=2 {
            for y in 0..=2 {
                result.push(Rect::new(hdivisions[x], vdivisions[y], hdivisions[x+1], vdivisions[y+1]))
            }    
        }

        result
    }

    fn subdivide_dimension(&self, intersection: &Cuboid, dim: fn(&Point) -> i32) -> Vec<i32> {
        vec![
            dim(&self.p1),
            dim(&intersection.p1),
            dim(&intersection.p2),
            dim(&self.p2),
        ]
    }

    fn is_3d(&self) -> bool {
        self.p1.x != self.p2.x && self.p1.y != self.p2.y && self.p1.z != self.p2.z
    }
}

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct Instruction {
    on: bool,
    x: Range,
    y: Range,
    z: Range,
}

#[derive(Debug, Hash, PartialEq, Eq, Clone, Copy)]
struct Range {
    min: i32,
    max: i32,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<Instruction> {
    input.lines().map(parse_instruction).collect()
}

fn parse_instruction(line: &str) -> Instruction {
    let parts = line.split_once(" ").unwrap();

    let ranges = parts.1.split("=").collect_vec();

    Instruction {
        on: parts.0 == "on",
        x: parse_range(&ranges[1][..ranges[1].len() - 2]),
        y: parse_range(&ranges[2][..ranges[2].len() - 2]),
        z: parse_range(&ranges[3][..ranges[3].len()]),
    }
}

fn parse_range(input: &str) -> Range {
    let parts = input.split_once("..").unwrap();

    Range { 
        min: parts.0.parse().unwrap(), 
        max: parts.1.parse().unwrap()
    }
}

fn generate_cuboids(instructions: &[Instruction]) -> Vec<CuboidInstruction> {
    instructions.iter().map(create_cuboid).collect()
}

fn create_cuboid(instruction: &Instruction) -> CuboidInstruction {
    CuboidInstruction {
        on: instruction.on, 
        cuboid: Cuboid::new( 
            Point{x: instruction.x.min, y: instruction.y.min, z: instruction.z.min},
            Point{x: instruction.x.max + 1, y: instruction.y.max + 1, z: instruction.z.max + 1},
        )
    }
}

fn part1(instructions: &[Instruction]) -> u64 {
    let cuboids = compute_lit_cuboids(&generate_cuboids(instructions));

    let viewport = Cuboid::new(Point::new(-50, -50, -50), Point::new(51, 51, 51));

    cuboids.iter()
        .map(|c| c.intersect(&viewport))
        .filter(|intersection| intersection.is_some())
        .map(|intersection| intersection.unwrap().area)
        .sum()
}

fn part2(instructions: &[Instruction]) -> u64 {
    compute_lit_cuboids(&generate_cuboids(instructions)).iter()
        .map(|c| c.area)
        .sum()
}

fn compute_lit_cuboids(instructions: &[CuboidInstruction]) -> Vec<Cuboid> {
    let mut lit_cuboids = Vec::new();

    for instruction in instructions.iter() {
        let mut new_lit_cuboids = subtract(&lit_cuboids, &instruction.cuboid);

        if instruction.on {
            new_lit_cuboids.push(instruction.cuboid);
        } 

        lit_cuboids = new_lit_cuboids;
    }

    lit_cuboids
}

fn subtract(cuboids: &[Cuboid], subtract: &Cuboid) -> Vec<Cuboid> {
    let mut result = Vec::new();

    for cuboid in cuboids.iter() {
        result.append(&mut cuboid.subtract(&subtract));
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work_for_input() {
        let input = fs::read_to_string("input4.txt").unwrap();
        let values = parse_input(&input);
        let result = part1(&values);

        assert_eq!(474140, result);
    }

    #[test]
    fn part2_should_work_for_input() {
        let input = fs::read_to_string("input4.txt").unwrap();
        let values = parse_input(&input);
        let result = part2(&values);

        assert_eq!(2758514936282235, result);
    }

    #[test]
    fn cuboid_intersect_should_work() {
        let cube1 = Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 3, y: 3, z: 3},
        );

        let cube2 = Cuboid::new(
            Point {x: 2, y: 2, z: 2},
            Point {x: 4, y: 4, z: 4},
        );

        let intersection = cube1.intersect(&cube2).unwrap();

        assert_eq!(2, intersection.p1.x);
        assert_eq!(2, intersection.p1.y);
        assert_eq!(2, intersection.p1.z);
        assert_eq!(3, intersection.p2.x);
        assert_eq!(3, intersection.p2.y);
        assert_eq!(3, intersection.p2.z);
    }

    #[test]
    fn cuboid_area_should_work() {
        let cube = Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 3, y: 3, z: 3},
        );

        assert_eq!(2*2*2, cube.area);
    }

    #[test]
    fn cuboid_cut_right_side() {
        let cube1 = Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 4, y: 4, z: 4},
        );

        let cube2 = Cuboid::new(
            Point {x: 2, y: 1, z: 1},
            Point {x: 4, y: 4, z: 4},
        );

        let cut_results = cube1.subtract(&cube2);

        assert_eq!(1, cut_results.len());
        assert_eq!(&Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 2, y: 4, z: 4},
        ), &cut_results[0]);
    }

    #[test]
    fn cuboid_cut_center_top() {
        let cube1 = Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 4, y: 4, z: 4},
        );

        let cube2 = Cuboid::new(
            Point {x: 2, y: 1, z: 2},
            Point {x: 3, y: 4, z: 3},
        );

        let cut_results = cube1.subtract(&cube2);
        eprintln!("cut_results = {:#?}", cut_results);
        assert_eq!(8, cut_results.len());
    }

    #[test]
    fn cuboid_cut_center_front() {
        let cube1 = Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 4, y: 4, z: 4},
        );
    
        let cube2 = Cuboid::new(
            Point {x: 2, y: 2, z: 1},
            Point {x: 3, y: 3, z: 4},
        );
    
        let cut_results = cube1.subtract(&cube2);
        assert_eq!(8, cut_results.len());
    }

    #[test]
    fn cuboid_cut_center_side() {
        let cube1 = Cuboid::new(
            Point {x: 1, y: 1, z: 1},
            Point {x: 4, y: 4, z: 4},
        );
    
        let cube2 = Cuboid::new(
            Point {x: 1, y: 2, z: 2},
            Point {x: 4, y: 3, z: 3},
        );
    
        let cut_results = cube1.subtract(&cube2);
        assert_eq!(8, cut_results.len());
    }

    #[test]
    fn parse_instruction_should_work() {
        let result = parse_instruction("on x=1..2,y=10..20,z=100..200");

        assert_eq!(true, result.on);
        assert_eq!(1, result.x.min);
        assert_eq!(2, result.x.max);
        assert_eq!(10, result.y.min);
        assert_eq!(20, result.y.max);
        assert_eq!(100, result.z.min);
        assert_eq!(200, result.z.max);
    }
}