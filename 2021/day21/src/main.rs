use std::{fs, collections::HashMap};
use itertools::{Itertools, iproduct};

#[derive(Debug)]
struct Player {
    location: i32,
    score: i32
}

#[derive(Debug)]
struct Die {
    value: i32,
    rolls: i32,
}

fn main() {
    let input = fs::read_to_string("input.txt").unwrap();
    let values = parse_input(&input);

    let part1 = part1(&values);
    let part2 = part2(&values);

    println!("Part 1: {} Part 2: {}", part1, part2);
}

fn parse_input(input: &str) -> Vec<i32> {
    input.lines().map(|line|line[28..].parse().unwrap()).collect()
}

fn part1(values: &[i32]) -> i32 {
    let mut player1 = Player { location: values[0] - 1, score: 0};
    let mut player2 = Player { location: values[1] - 1, score: 0};
    let mut die = Die {value: 100, rolls: 0};

    eprintln!("player1 = {:?}", player1);
    eprintln!("player2 start = {:?}", player2);

    loop {
        player_move(&mut player1, &mut die);
        if player1.score >= 1000 {
            return die.rolls * player2.score;
        }
        player_move(&mut player2, &mut die);
        if player2.score >= 1000 {
            return die.rolls * player1.score;
        }
    }

    1
}

fn player_move(player: &mut Player, die: &mut Die) {
    let die_moves = throw(die) + throw(die) + throw(die);

    player.location = (player.location + die_moves) % 10;
    player.score += player.location + 1;
}

fn throw(die: &mut Die) -> i32 {
    if die.value == 100 {
        die.value = 1;
    } else {
        die.value += 1;
    }

    die.rolls += 1;
    die.value
}

fn part2(values: &[i32]) -> usize {
    let (s1,s2) = quantum_game(&mut HashMap::new(),0,0,4,2);

    eprintln!("s1 = {:?}", s1);
    eprintln!("s2 = {:?}", s2);

    s1.max(s2)
}

type Cache = HashMap<(usize,usize,usize,usize),(usize,usize)>;

fn quantum_game(cache: &mut Cache, score1: usize, score2: usize, position1: usize, position2: usize) -> (usize,usize) {
    if score2 >= 21 { return (0,1); }
    if let Some(&score) = cache.get(&(score1,score2,position1,position2)) { return score; }
  
    let mut score = (0,0);
    for (d1,d2,d3) in iproduct!([1,2,3],[1,2,3],[1,2,3]) {
      let die = d1+d2+d3;
      let position1 = position1 + die - if position1+die > 10 {10} else {0};
      let (s1,s2) = quantum_game(cache,score2,score1+position1,position2,position1);
      score.0 += s2;
      score.1 += s1;
    }
  
    cache.insert((score1,score2,position1,position2), score);
    score
  }

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn part1_should_work() {
        let input = parse_input(&fs::read_to_string("input.txt").unwrap());
        let result = part1(&input);

        assert_eq!(908595, result);
    }

    #[test]
    fn part2_should_work() {
        let input = parse_input(&fs::read_to_string("input.txt").unwrap());
        let result = part2(&input);

        assert_eq!(91559198282731, result);
    }
}