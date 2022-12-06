use std::{fs, collections::HashMap};
use itertools::Itertools;

#[derive(Debug)]
struct Node {
    name: String,
    connections: Vec<String>,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);
    
    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> HashMap<String, Node> {
    let connections: Vec<Vec<String>> = input.lines().map(|line| line.split('-').map(String::from).collect_vec()).collect();
    let mut nodes: HashMap<String, Node> = connections.iter()
        .flat_map(|c| c).unique()
        .map(|name| (name.clone(), Node {name: name.clone(), connections: Vec::new()}))
        .collect();
    
    for connection in connections.iter() {
        connect_node(&connection[0], &connection[1], &mut nodes);
        connect_node(&connection[1], &connection[0], &mut nodes);
    }

    nodes
}

fn connect_node(node1: &str, node2: &str, nodes: &mut HashMap<String, Node>) {
    let node = nodes.get_mut(node1).unwrap();
    node.connections.push(String::from(node2));
}

fn part1(nodes: &HashMap<String, Node>) -> usize {
    find_paths(nodes, false).len()
}

fn part2(nodes: &HashMap<String, Node>) -> usize {
    find_paths(nodes, true).len()
}

fn find_paths(nodes: &HashMap<String, Node>, allow_multiple_visits: bool) -> Vec<Vec<String>> {
    let mut path = vec![String::from("start")];
    let mut paths = Vec::new();

    search_node(nodes, &mut path, &mut paths, allow_multiple_visits);

    paths
}

fn search_node(nodes: &HashMap<String, Node>, path: &mut Vec<String>, paths: &mut Vec<Vec<String>>, allow_multiple_visits: bool) {
    let node = nodes.get(path.last().unwrap()).unwrap();
    if node.name == "end" {
        paths.push(path.clone());
        return;
    }

    for next_node in node.connections.iter() {
        if can_visit_node(&next_node, path, allow_multiple_visits) {
            path.push(next_node.clone());
            search_node(nodes, path, paths, allow_multiple_visits);
            path.pop();
        }
    }
}

fn can_visit_node(name: &String, path: &Vec<String>, allow_multiple_visits: bool) -> bool {
    if is_large_cave(name) || !path.contains(name) {
        return true;
    }

    // We have a small cave that we have visited before
    if allow_multiple_visits && !is_start_or_end(name) && !visited_small_cave_twice(path) {
        true
    } else {
        false
    }
}

fn is_large_cave(name: &str) -> bool {
    !is_small_cave(name)
}

fn is_small_cave(name: &str) -> bool {
    name.chars().all(|c| c.is_lowercase())
}

fn visited_small_cave_twice(path: &Vec<String>) -> bool {
    path.iter()
        .filter(|n| is_small_cave(n)).counts().values()
        .any(|count| *count > 1)
}

fn is_start_or_end(name: &str) -> bool {
    name == "start" || name ==  "end"
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let values = parse_input(&input);
        let result = part1(&values);

        assert_eq!(3679, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let values = parse_input(&input);
        let result = part2(&values);

        assert_eq!(36, result);
    }
}