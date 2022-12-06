use std::{fs, vec};
use itertools::Itertools;

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let variables = get_values_from_program(&input.lines().collect_vec());
    eprintln!("variables = {:?}", variables);

    let part1 = solve_part1(&variables);
    let part2 = solve_part2(&variables);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

// fn original_program() {
//     let add1 =  vec![10, 11, 14, 13, -6, -14, 14, 13, -8, -15, 10, -11, -13, -4];
//     let add2 =  vec![1,  9,  12, 6,   9,  15,  7, 12, 15,  3,   6,   2,  10, 12];
//     let div =   vec![1,  1,  1,  1,  26,  26,  1,  1, 26, 26,   1,  26,  26, 26];
//     let input = vec![9,  9,  9,  9,   9,   9,  9,  9,  9,  9,   9,   9,  9,  9];
//     let input = vec![9,  9,  9,  9,   9,   7,  9,  5,  9,  1,   9,   4,  5,  6];
//     let input = vec![4,  5,  3,  1,   1,   1,  9,  1,  5,  1,   6,   1,  1,  1];
//     let mut stack = vec![];

//     for i in 0..add1.len() {
//         let w = input[i];

//         if *stack.last().unwrap_or(&0) == w - add1[i] {
//             if div[i] == 26 {
//                 stack.pop();
//             }
//         } else {
//             if div[i] == 26 {
//                 stack.pop();
//             }

//             stack.push(w + add2[i]);
//         }
//     }
//     eprintln!("stack = {:?}", stack);


//     let z = stack.iter().fold(0, |acc, current| (acc * 26) + current);

//     eprintln!("z = {:?}", z);
// }

fn solve_part1(variables: &ProgramVariables) -> String {
    let input = vec![9;14];
    let result = solve(variables, &input, &balance_up);

    result.iter().map(|v| v.to_string()).collect()
}

fn solve_part2(variables: &ProgramVariables) -> String{
    let input = vec![1;14];
    let result = solve(variables, &input, &balance_down);

    result.iter().map(|v| v.to_string()).collect()
}

fn balance_up(pair: BalancePair, input: &mut Vec<i32>) {
    let sum_push = pair.push.add2 + input[pair.push.index];
    let sum_pop = input[pair.pop.index] - pair.pop.add1;

    if sum_pop < sum_push {
        input[pair.push.index] = 1 + (sum_push - sum_pop);
    } else {
        input[pair.pop.index] = sum_push + pair.pop.add1;
    }
}

fn balance_down(pair: BalancePair, input: &mut Vec<i32>) {
    let sum_push = pair.push.add2 + input[pair.push.index];
    let sum_pop = input[pair.pop.index] - pair.pop.add1;

    if sum_pop < sum_push {
        input[pair.pop.index] = sum_push + pair.pop.add1; 
    } else {
        input[pair.push.index] = 1 + (sum_pop - sum_push);
    }
}

struct Frame {
    add1: i32,
    add2: i32,
    index: usize,
}

struct BalancePair {
    push: Frame,
    pop: Frame,
}

fn solve<F>(variables: &ProgramVariables, input: &Vec<i32>, balance: F) -> Vec<i32> where
        F: Fn(BalancePair, &mut Vec<i32>) -> () {

    let mut input = input.clone();
    let mut stack = vec![];

    for i in 0..variables.add1.len() {
        if variables.div[i] == 26 {
            let push_index = stack.pop().unwrap();

            balance(BalancePair {
                push: Frame { 
                    add1: variables.add1[push_index], 
                    add2: variables.add2[push_index], 
                    index: push_index
                },
                pop: Frame { 
                    add1: variables.add1[i], 
                    add2: variables.add2[i], 
                    index: i
                }
            }, &mut input);
        } else {
            stack.push(i);
        }
    }

    input
}

#[derive(Debug)]
struct ProgramVariables {
    add1: Vec<i32>,
    add2: Vec<i32>,
    div: Vec<i32>,
}

fn get_values_from_program(lines: &Vec<&str>) -> ProgramVariables {
    ProgramVariables {
        div: (0..14).map(|i| get_last_operand(&lines[(i * 18) + 4])).collect(),
        add1: (0..14).map(|i| get_last_operand(&lines[(i * 18) + 5])).collect(),
        add2: (0..14).map(|i| get_last_operand(&lines[(i * 18) + 15])).collect(),
    }
}

fn get_last_operand(input: &str) -> i32 {
    let parts = input.split_whitespace().collect_vec();

    parts.last().unwrap().parse().unwrap()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let variables = get_values_from_program(&input.lines().collect_vec());
        let result = solve_part1(&variables);

        assert_eq!("99999795919456", result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input.txt").unwrap();
        let variables = get_values_from_program(&input.lines().collect_vec());
        let result = solve_part2(&variables);

        assert_eq!("45311191516111", result);
    }
}