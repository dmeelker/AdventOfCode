use std::fs;

struct Command {
    command: String,
    value: i32,
}

fn main() {
    let commands = parse_input("input.txt");
    let part1 = part1(&commands);
    let part2 = part2(&commands);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(file: &str) -> Vec<Command> {
    let input = fs::read_to_string(file).unwrap();
    input.lines().map(parse_input_line).collect()
}

fn parse_input_line(line: &str) -> Command {
    let parts: Vec<&str> = line.split(' ').collect();
    Command { 
        command: String::from(parts[0]),
        value: parts[1].parse().unwrap() 
    }
}

fn part1(commands: &[Command]) -> i32 {
    let mut x = 0;
    let mut depth = 0;

    for command in commands.iter() {
        match command.command.as_str() {
            "forward" => x += command.value,
            "down" => depth += command.value,
            "up" => depth -= command.value,
            _ => panic!()
        }
    }

    x * depth
}

fn part2(commands: &[Command]) -> i32 {
    let mut x = 0;
    let mut depth = 0;
    let mut aim = 0;

    for command in commands.iter() {
        match command.command.as_str() {
            "forward" =>  {
                x += command.value;
                depth += aim * command.value;
            },
            "down" => aim += command.value,
            "up" => aim -= command.value,
            _ => panic!()
        }
    }

    x * depth
}