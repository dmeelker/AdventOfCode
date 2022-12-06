use itertools::Itertools;

#[derive(Debug, Clone)]
pub struct Cell {
    pub value: i32,
    pub marked: bool,
}

#[derive(Debug, Clone)]
pub struct Board {
    pub width: usize,
    pub height: usize,
    data: Vec<Cell>
}

impl Board {
    pub fn new(width: usize, height: usize, values: &[i32]) -> Board {
        if values.len() != width * height {
            panic!();
        }

        let data = values.iter().map(|v| Cell {value: *v, marked: false}).collect_vec();
        Board {width, height, data}
    }

    pub fn get(&self, x: usize, y: usize) -> &Cell {
        &self.data[(y * self.width) + x]
    }

    pub fn mark(&mut self, value: i32) -> bool {
        let cell = self.data.iter_mut().find(|cell| cell.value == value);

        if let Some(cell) = cell{
            cell.marked = true;
            true
        } else {
            false
        }
    }

    pub fn winner(&self) -> bool {
        self.has_full_row() || self.has_full_column()
    }

    fn has_full_row(&self) -> bool {
        (0..self.height).any(|y| 
            (0..self.width).all(|x| self.get(x, y).marked)
        )
    }

    fn has_full_column(&self) -> bool {
        (0..self.width).any(|x| 
            (0..self.height).all(|y| self.get(x, y).marked)
        )
    }

    pub fn sum_unmarked(&self) -> i32 {
        self.data.iter().filter(|c| !c.marked).map(|c| c.value).sum()
    }
}