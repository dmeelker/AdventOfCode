use std::{fs, collections::{HashMap, HashSet}};
use board::{Unit, Board, NodeLink, NodeId};
use itertools::Itertools;

mod board;


fn main() {
    let input = fs::read_to_string("input2.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&input);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<String> {
    input.lines().map(String::from).collect()
}

fn part1(input: &str) -> usize {
    let mut space = board::build_empty_board();
    space.populate(input);

    let mut path = Vec::new();
    let mut solutions = Vec::new();
    path.push(Move {new_state: space.clone(), cost: 0});

    let mut lowest_cost = usize::MAX;
    let mut move_cache = HashSet::new();
    
    find_solution_step(&space, &mut path, 0, &mut solutions, &mut lowest_cost, &mut move_cache);
    
    let solutions = solutions.iter().sorted_by(|a,b| a.cost.cmp(&b.cost)).collect_vec();
    println!("Found {} solutions", solutions.len());

    for m in solutions[0].path.iter() {
        m.new_state.print();
        println!("Cost: {}", m.cost);
        println!();
    }
    // for s in solutions.iter() {
    //     s.path.last().unwrap().new_state.print();
    //     println!("Cost: {}", s.cost);
    // }

    solutions[0].cost
}


#[derive(Clone, Debug, PartialEq, Eq, Hash)]
struct Move {
    new_state: Board,
    cost: usize,
}

#[derive(Clone, Debug, PartialEq, Eq, Hash)]
struct Solution {
    path: Vec<Move>,
    cost: usize,
}

// 18357 high
fn find_solution_step(board: &Board, path: &mut Vec<Move>, path_cost: usize, solutions: &mut Vec<Solution>, lowest_cost: &mut usize, move_cache: &mut HashSet<Vec<Move>>) {
    if move_cache.contains(&path) || path_cost > *lowest_cost {
        return;
    }

    move_cache.insert(path.clone());
    
    // if solutions.len() == 14808 {
    //     return;
    // }
    //board.print();
    //println!();

    if board.won() {
        let solution = Solution { path: path.clone(), cost: path_cost };
        *lowest_cost = (*lowest_cost).min(solution.cost);
        solutions.push(solution);
        
        println!("{}", solutions.len());
        //println!("{}", solutions.len());
        // if solutions.len() > 10000 - 3 {
        //     for m in solutions.last().unwrap().path.iter() {
        //         m.new_state.print();
        //         println!("Cost: {}", m.cost);
        //         println!();
        //     }
        //     println!("====");
        // }

        if solutions.len() == 200000 {
            for m in solutions.last().unwrap().path.iter() {
                m.new_state.print();
                println!("Cost: {}", m.cost);
                println!();
            }
            panic!();
        }
        return;
    }

    let unit_moves = board.get_units_with_moves();
    //let occupied = board.get_units_to_move();

    for (unit, moves) in unit_moves.iter() {
        // let moves = match move_cache.get(&(board.clone(), *unit)) {
        //     Some(moves) => moves.clone(),
        //     None => {
        //         let moves = board.get_available_moves(*unit);
        //         move_cache.insert((board.clone(), *unit), moves.clone());
        //         moves
        //     }
        // };

        

        //let moves = board.get_available_moves(*unit);

        //if let Some(moves) = moves {
            for m in moves.iter() {
                let mut new_state = board.clone();
                new_state.move_unit(*unit, m.id);

                path.push(Move {new_state: new_state.clone(), cost: m.cost});
                
                find_solution_step(&new_state, path, path_cost + m.cost, solutions, lowest_cost, move_cache);

                path.pop();
            }
        //}
    }
}

fn part2(values: &[String]) -> i32 {
    2
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = vec![String::from("123")];
        let result = part1(&input);

        assert_eq!(1, result);
    }

    #[test]
    fn part2_should_work() {
        let input = vec![String::from("123")];
        let result = part2(&input);

        assert_eq!(2, result);
    }
}