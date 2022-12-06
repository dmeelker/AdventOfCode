use std::vec;

use itertools::Itertools;

pub type Unit = char;
pub type NodeId = usize;

#[derive(Debug, Clone,PartialEq, Eq, Hash)]
pub struct Board {
    nodes: Vec<Node>,
    pub hallway: Vec<NodeId>,
    pub room_a: Vec<NodeId>,
    pub room_b: Vec<NodeId>,
    pub room_c: Vec<NodeId>,
    pub room_d: Vec<NodeId>,
}

impl Board {
    fn create_node(&mut self) -> NodeId {
        let id = self.nodes.len();
        self.nodes.push(Node { id, occupant: None, target: None, neighbours: Vec::new()});

        id
    }

    pub fn get(&self, id: NodeId) -> &Node {
        self.nodes.get(id).unwrap()
    }

    pub fn get_mut(&mut self, id: NodeId) -> &mut Node {
        self.nodes.get_mut(id).unwrap()
    }

    pub fn move_unit(&mut self, from: NodeId, to: NodeId) {
        let unit = self.nodes[from].occupant.unwrap();

        self.nodes[from].occupant = None;
        self.nodes[to].occupant = Some(unit);
    }

    pub fn print(&self) {
        println!("#############");

        println!("#{}{}.{}.{}.{}.{}{}#", 
            self.get_occupant(self.hallway[0]), 
            self.get_occupant(self.hallway[1]),
            self.get_occupant(self.hallway[2]), 
            self.get_occupant(self.hallway[3]), 
            self.get_occupant(self.hallway[4]), 
            self.get_occupant(self.hallway[5]), 
            self.get_occupant(self.hallway[6]));

        println!("###{}#{}#{}#{}###", 
            self.get_occupant(self.room_a[0]), 
            self.get_occupant(self.room_b[0]), 
            self.get_occupant(self.room_c[0]), 
            self.get_occupant(self.room_d[0]));

        println!("  #{}#{}#{}#{}#  ", 
            self.get_occupant(self.room_a[1]), 
            self.get_occupant(self.room_b[1]), 
            self.get_occupant(self.room_c[1]), 
            self.get_occupant(self.room_d[1]));

        println!("  #########  ");
    }

    fn get_occupant(&self, id: NodeId) -> String {
        let node = self.get(id);

        if let Some(occupant) = node.occupant {
            occupant.to_string()
        } else {
            String::from(".")
        }
    }

    fn clear(&mut self) {
        for node in self.nodes.iter_mut() {
            node.occupant = None;
        }
    }

    pub fn populate(&mut self, input: &str) {
        self.clear();

        let lines = input.lines().collect_vec();

        self.get_mut(self.room_a[0]).occupant = Some(lines[2].chars().nth(3).unwrap());
        self.get_mut(self.room_a[1]).occupant = Some(lines[3].chars().nth(3).unwrap());

        self.get_mut(self.room_b[0]).occupant = Some(lines[2].chars().nth(5).unwrap());
        self.get_mut(self.room_b[1]).occupant = Some(lines[3].chars().nth(5).unwrap());

        self.get_mut(self.room_c[0]).occupant = Some(lines[2].chars().nth(7).unwrap());
        self.get_mut(self.room_c[1]).occupant = Some(lines[3].chars().nth(7).unwrap());

        self.get_mut(self.room_d[0]).occupant = Some(lines[2].chars().nth(9).unwrap());
        self.get_mut(self.room_d[1]).occupant = Some(lines[3].chars().nth(9).unwrap());
    }

    pub fn get_units_with_moves(&self) -> Vec<(NodeId, Vec<NodeLink>)> {
        let mut result = Vec::new();
        let moveable = self.get_units_to_move();

        for m in moveable.iter() {
            if let Some(moves) = self.get_available_moves(*m) {
                result.push((*m, moves));
            }
        }

        result
    }

    pub fn get_units_to_move(&self) -> Vec<NodeId> {
        self.nodes.iter().filter(|n| n.occupant.is_some()).map(|n| n.id).filter(|id| !self.unit_in_final_destination(*id)).collect()
    }

    pub fn get_available_moves(&self, id: NodeId) -> Option<Vec<NodeLink>> {
        let node = self.get(id);
        if node.occupant.is_none() {
            return None;
        }

        if self.is_hallway(id) {
            let room_target = self.find_available_room(node.occupant.unwrap());
            if room_target.is_none() {
                return None;
            }

            let room_target = room_target.unwrap();

            let path = self.find_path(id, room_target);

            if let Some(path) = path {
                let cost = self.get_path_cost(&path) * unit_move_cost(node.occupant.unwrap());
                return Some(vec![NodeLink {id: room_target, cost}]);
            } else {
                return None;
            }
        } else {
            if self.unit_in_final_destination(id) {
                return None;
            }

            let mut paths = Vec::new();

            for hallway_id in self.hallway.iter() {
                let hallway = self.get(*hallway_id);

                if hallway.occupant.is_none() {
                    let path = self.find_path(id, *hallway_id);

                    if let Some(path) = path {
                        let cost = self.get_path_cost(&path) * unit_move_cost(node.occupant.unwrap());
                        paths.push(NodeLink {id: *hallway_id, cost});
                    }
                }
            }

            return Some(paths);
        }
    }

    fn find_path(&self, from: NodeId, to: NodeId) -> Option<Vec<NodeId>> {
        let mut path = Vec::new();
        let mut paths = Vec::new();

        self.find_path_node(from, to, &mut path, &mut paths);
        
        if !paths.is_empty() {
            let path = paths.iter().sorted_by(|a,b| a.len().cmp(&b.len())).next().unwrap().clone();
            Some(path)
        } else {
            None
        }
    }

    fn find_path_node(&self, id: NodeId, target_id: NodeId, path: &mut Vec<NodeId>, paths: &mut Vec<Vec<NodeId>>) {
        let node = self.get(id);
        
        path.push(node.id);

        if id == target_id {
            paths.push(path.clone());
            path.pop();
            return;
        }

        for neighbour in node.neighbours.iter() {
            let neighbour_node = self.get(neighbour.id);

            if neighbour_node.occupant.is_some() || path.contains(&neighbour.id) {
                continue;
            }

            self.find_path_node(neighbour_node.id, target_id, path, paths)
        }

        path.pop();
    }

    fn find_available_room(&self, unit: Unit) -> Option<NodeId> {
        let room = self.get_room_for_unit(unit);
        let occupants = room.iter().map(|id| self.nodes[*id].occupant).collect_vec();

        if occupants[0].is_some() {
            return None;
        }

        if occupants[1].is_none() {
            return Some(room[1]);
        }

        if self.unit_in_final_destination(room[1]) {
            return Some(room[0]);
        }

        return None;
    }

    fn get_room_for_unit(&self, unit: Unit) -> &Vec<NodeId> {
        match unit {
            'A' => &self.room_a,
            'B' => &self.room_b,
            'C' => &self.room_c,
            'D' => &self.room_d,
            _ => panic!()
        }
    }

    fn get_path_cost(&self, path: &[NodeId]) -> usize {
        let mut node = self.get(path[0]);
        let mut cost = 0;

        for next_id in path.iter().skip(1) {
            let link = node.neighbours.iter().find(|n| n.id == *next_id).unwrap();
            cost += link.cost;
            node = self.get(*next_id);
        }

        cost
    }
    
    fn unit_in_final_destination(&self, id: NodeId) -> bool {
        let node = &self.nodes[id];
        let room = self.get_room_for_unit(node.occupant.unwrap());

        (room[0] == id && self.unit_in_correct_room(room[1])) || room[1] == id
    }

    fn unit_in_correct_room(&self, id: NodeId) -> bool {
        let node = &self.nodes[id];
        let room = self.get_room_for_unit(node.occupant.unwrap());

        room.contains(&id)
    }

    fn is_hallway(&self, id: NodeId) -> bool {
        self.hallway.iter().any(|n| *n == id)
    }

    fn is_room(&self, id: NodeId) -> bool {
        !self.is_hallway(id)
    }

    pub fn won(&self) -> bool {
        self.nodes.iter()
            .filter(|n| n.occupant.is_some())
            .all(|n| self.unit_in_final_destination(n.id))
    }
}

fn unit_move_cost(unit: Unit) -> usize {
    match unit {
        'A' => 1,
        'B' => 10,
        'C' => 100,
        'D' => 1000,
        _ => panic!()
    }
}

/*
#############
#...........#
###A#B#C#D###
  #A#B#C#D#
  #########
*/

#[derive(Debug, Clone,PartialEq, Eq, Hash)]
pub struct Node {
    pub id: NodeId,
    pub occupant: Option<Unit>,
    pub target: Option<Unit>,
    pub neighbours: Vec<NodeLink>,
}

#[derive(Debug, Clone,PartialEq, Eq, Copy, Hash)]
pub struct NodeLink {
    pub id: NodeId,
    pub cost: usize,
}

pub fn build_empty_board() -> Board {
    let mut board = Board {
        nodes: Vec::new(),
        hallway: Vec::new(),
        room_a: Vec::new(),
        room_b: Vec::new(),
        room_c: Vec::new(),
        room_d: Vec::new(),
    };

    let mut room_a = create_room_nodes('A', &mut board);
    let mut room_b = create_room_nodes('B', &mut board);
    let mut room_c = create_room_nodes('C', &mut board);
    let mut room_d = create_room_nodes('D', &mut board);
    board.hallway = create_hallway(&mut board);

    board.room_a.append(&mut room_a);
    board.room_b.append(&mut room_b);
    board.room_c.append(&mut room_c);
    board.room_d.append(&mut room_d);

    link_nodes(board.hallway[1], board.room_a[0], 2, &mut board);
    link_nodes(board.hallway[2], board.room_a[0], 2, &mut board);

    link_nodes(board.hallway[2], board.room_b[0], 2, &mut board);
    link_nodes(board.hallway[3], board.room_b[0], 2, &mut board);

    link_nodes(board.hallway[3], board.room_c[0], 2, &mut board);
    link_nodes(board.hallway[4], board.room_c[0], 2, &mut board);

    link_nodes(board.hallway[4], board.room_d[0], 2, &mut board);
    link_nodes(board.hallway[5], board.room_d[0], 2, &mut board);

    board
}

fn create_room_nodes(target: Unit, board: &mut Board) -> Vec<NodeId> {
    let id1 = board.create_node();
    let id2 = board.create_node();

    board.get_mut(id1).target = Some(target);
    board.get_mut(id2).target = Some(target);

    link_nodes(id1, id2, 1, board);

    vec![id1, id2]
}

fn create_hallway(board: &mut Board) -> Vec<NodeId> {
    let mut ids = Vec::new();

    for _ in 0..7 {
        ids.push(board.create_node());
    }

    link_nodes(ids[0], ids[1], 1, board);
    link_nodes(ids[1], ids[2], 2, board);
    link_nodes(ids[2], ids[3], 2, board);
    link_nodes(ids[3], ids[4], 2, board);
    link_nodes(ids[4], ids[5], 2, board);
    link_nodes(ids[5], ids[6], 1, board);

    ids
}

fn link_nodes(id1: NodeId, id2: NodeId, cost: usize, board: &mut Board) {
    board.get_mut(id1).neighbours.push(NodeLink { id: id2, cost});
    board.get_mut(id2).neighbours.push(NodeLink { id: id1, cost});
}

