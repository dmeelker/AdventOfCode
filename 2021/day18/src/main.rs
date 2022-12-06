use std::fs;
use itertools::Itertools;

#[derive(Debug, Clone, Copy, PartialEq, Eq)]
struct Number {
    value: i32,
    depth: i32,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<String> {
    input.lines().map(String::from).collect()
}

fn part1(lines: &[String]) -> usize {
    let mut numbers = lines.iter().map(|line| parse_numbers(line)).collect_vec();
    let mut added = numbers.remove(0);

    while !numbers.is_empty() {
        let to_add = numbers.remove(0);
        added = add_and_reduce(&added, &to_add);
    }

    magnitude(&added)
}

fn part2(lines: &[String]) -> usize {
    let numbers = lines.iter().map(|line| parse_numbers(line)).collect_vec();

    let mut largest_magnitude = 0;

    for left_number in numbers.iter().enumerate() {
        for right_number in numbers.iter().enumerate() {
            if left_number.0 == right_number.0 {
                continue;
            }

            let result = add_and_reduce(left_number.1, right_number.1);
            let magnitude = magnitude(&result);

            largest_magnitude = largest_magnitude.max(magnitude);
        }    
    }

    largest_magnitude
}

fn parse_numbers(input: &str) -> Vec<Number> {
    let mut numbers = Vec::new();
    let mut accumulator = String::new();
    let mut depth = 0;

    for chr in input.chars() {
        if chr.is_digit(10) {
            accumulator.push_str(&chr.to_string());
        } else if !accumulator.is_empty() {
            numbers.push(Number { depth, value: accumulator.to_string().parse().unwrap()});
            accumulator.clear();
        }

        if chr == '[' {
            depth += 1;
        } else if chr == ']' {
            depth -= 1;
        }
    }

    numbers
}

fn add_and_reduce(left: &[Number], right: &[Number]) -> Vec<Number> {
    let result = add(left, right);
    reduce(&result)
}

fn add(left: &[Number], right: &[Number]) -> Vec<Number> {
    left.iter().map(clone_number_deeper).chain(
        right.iter().map(clone_number_deeper)
    ).collect()
}

fn clone_number_deeper(number: &Number) -> Number {
    Number { value: number.value, depth: number.depth + 1}
}

fn reduce(numbers: &[Number]) -> Vec<Number> {
    let mut numbers = numbers.iter().copied().collect_vec();

    loop {
        loop {
            let old_numbers = numbers.iter().copied().collect_vec();
            numbers = explode(&numbers);

            if numbers.eq(&old_numbers) {
                break;
            }
        }

        let old_numbers = numbers.iter().copied().collect_vec();
        numbers = split(&numbers);

        if numbers.eq(&old_numbers) {
            break;
        }
    }

    numbers
}

fn split(numbers: &[Number]) -> Vec<Number> {
    let mut result = numbers.iter().copied().collect_vec();
    let position = result.iter().position(|number| number.value >= 10);

    if let Some(position) = position {
        let number = result.remove(position);
        result.insert(position, Number {depth: number.depth + 1, value: (number.value as f32 / 2.0).floor() as i32});
        result.insert(position + 1, Number {depth: number.depth + 1, value: (number.value as f32 / 2.0).ceil() as i32});
    }

    result
}

fn explode(numbers: &[Number]) -> Vec<Number> {
    let mut result: Vec<Number> = numbers.iter().copied().collect_vec();
    let position = result.iter().position(|number| number.depth == 5);

    if let Some(i) = position {
        let left = result[i];
        let right = result[i+1];

        if i > 0 {
            result[i-1].value += left.value;
        }

        result[i] = Number {depth: left.depth - 1, value: 0};

        if i < result.len() - 2 {
            result[i+2].value += right.value;
        }

        result.remove(i+1); // Remove left value
    }

    result
}

fn magnitude(numbers: &[Number]) -> usize {
    let mut numbers = numbers.iter().copied().collect_vec();

    loop {
        let max_depth = numbers.iter().map(|n| n.depth).max().unwrap();
        if max_depth == 0 {
            break;
        }

        let mut i = 0;

        while i < numbers.len() {
            let depth = numbers[i].depth;

            if depth == max_depth {
                let left = numbers[i].value;
                let right = numbers[i+1].value;

                numbers.remove(i); // Left
                numbers.remove(i); // Right
                numbers.insert(i, Number {value: (left*3) + (right*2), depth: depth - 1});
            }

            i+=1;
        }
    }

    numbers[0].value as usize
}

fn format_numbers(numbers: &[Number]) -> String {
    let mut numbers = numbers.iter().copied().collect_vec();
    format_numbers_pair(&mut numbers, 1)
}

fn format_numbers_pair(numbers: &mut Vec<Number>, depth: i32) -> String {
    let left = format_number_part(numbers, depth);
    let right = format_number_part(numbers, depth);

    return format!("[{},{}]", left, right);
}

fn format_number_part(numbers: &mut Vec<Number>, depth: i32) -> String {
    let number = numbers[0];
    if number.depth == depth {
        numbers.remove(0).value.to_string()
    } else {
        format_numbers_pair(numbers, depth + (number.depth - depth).signum())
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let input = parse_input(&input);
        let result = part1(&input);

        assert_eq!(4140, result);
    }

    #[test]
    fn part2_should_work() {
        let input = fs::read_to_string("input2.txt").unwrap();
        let input = parse_input(&input);
        let result = part2(&input);

        assert_eq!(3993, result);
    }

    #[test]
    fn parse_numbers_should_parse_single_level() {
        let numbers = parse_numbers("[1,2]");

        assert_eq!(2, numbers.len());
        assert_eq!(Number { value: 1, depth: 1}, numbers[0]);
        assert_eq!(Number { value: 2, depth: 1}, numbers[1]);
    }

    #[test]
    fn parse_numbers_should_parse_multiple_level() {
        let numbers = parse_numbers("[1,[2,3]]");

        assert_eq!(3, numbers.len());
        assert_eq!(Number { value: 1, depth: 1}, numbers[0]);
        assert_eq!(Number { value: 2, depth: 2}, numbers[1]);
        assert_eq!(Number { value: 3, depth: 2}, numbers[2]);
    }

    #[test]
    fn format_numbers_should_work() {
        test_format("[1,[2,3]]");
        test_format("[[1,2],[3,4]]");
        test_format("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]");
        test_format("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]");
        test_format("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]");
        test_format("[[[[4,0],[5,4]],[[7,0],[[7,8],5]]],[10,[[11,9],[11,0]]]]");
    }

    fn test_format(input: &str) {
        let numbers = parse_numbers(input);
        let result = format_numbers(&numbers);

        assert_eq!(input, result);
    }

    fn assert_numbers_eq(expected: &str, numbers: &[Number]) {
        assert_eq!(expected, format_numbers(&numbers));
    }

    #[test]
    fn numbers_add_should_work() {
        let left = parse_numbers("[1,2]");
        let right = parse_numbers("[[3,4],5]");

        let result = add(&left, &right);
        assert_numbers_eq("[[1,2],[[3,4],5]]", &result);
    }

    #[test]
    fn numbers_split_should_work() {
        let numbers = parse_numbers("[11,2]");
        let result = split(&numbers);
        assert_numbers_eq("[[5,6],2]", &result);
    }

    #[test]
    fn numbers_split_should_only_split_first() {
        let numbers = parse_numbers("[11,[10,5]]");
        let result = split(&numbers);
        assert_numbers_eq("[[5,6],[10,5]]", &result);
    }

    #[test]
    fn numbers_explode_should_work() {
        test_explode("[[[[0,9],2],3],4]", "[[[[[9,8],1],2],3],4]");
        test_explode("[7,[6,[5,[7,0]]]]", "[7,[6,[5,[4,[3,2]]]]]");
        test_explode("[[6,[5,[7,0]]],3]", "[[6,[5,[4,[3,2]]]],1]");
        test_explode("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]");
        test_explode("[[3,[2,[8,0]]],[9,[5,[7,0]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]");
    }

    fn test_explode(expected: &str, input: &str) {
        let numbers = parse_numbers(input);
        let result = explode(&numbers);
        assert_numbers_eq(expected, &result);
    }

    #[test]
    fn numbers_magnitude_should_work_for_single_pair() {
        let numbers = parse_numbers("[1,1]");
        let result = magnitude(&numbers);
        assert_eq!(5, result);
    }

    #[test]
    fn numbers_magnitude_should_work_for_nested_pairs() {
        let numbers = parse_numbers("[[1,1], [1,1]]");
        let result = magnitude(&numbers);
        assert_eq!(25, result);
    }

    #[test]
    fn reduce_should_work1() {
        let numbers = parse_numbers("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]");
        let result = reduce(&numbers);
        assert_numbers_eq("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", &result);
    }

    #[test]
    fn numbers_add_and_reduce_should_work() {
        let left = parse_numbers("[[[[4,3],4],4],[7,[[8,4],9]]]");
        let right = parse_numbers("[1,1]");
        let result = add_and_reduce(&left, &right);

        assert_eq!("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", format_numbers(&result));
    }

    #[test]
    fn numbers_add_and_reduce_should_work1() {
        let left = parse_numbers("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]");
        let right = parse_numbers("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]");
        let result = add_and_reduce(&left, &right);

        assert_eq!("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]", format_numbers(&result));
    }

    #[test]
    fn numbers_add_and_reduce_should_work2() {
        let left = parse_numbers("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]");
        let right = parse_numbers("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]");
        let result = add_and_reduce(&left, &right);

        assert_eq!("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]", format_numbers(&result));
    }
}

