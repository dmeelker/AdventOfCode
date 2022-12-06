use itertools::Itertools;

#[derive(Debug, Clone)]
pub struct Grid {
    pub data: Vec<Vec<bool>>,
}

impl Grid {
    pub fn width(&self) -> usize {
        self.data[0].len()
    }

    pub fn height(&self) -> usize {
        self.data.len()
    }

    pub fn grow(&self, size: usize, default: bool) -> Grid {
        let mut data = vec![];

        for _ in 0..size {
            data.push(std::iter::repeat(default).take(self.width() + (size * 2)).collect_vec());
        }

        for y in 0..self.height() {
            let mut row = vec![];
            for _ in 0..size {
                row.push(default);
            }

            for x in 0..self.width() {
                row.push(self.data[y][x]);
            }

            for _ in 0..size {
                row.push(default);
            }

            data.push(row);
        }

        for _ in 0..size {
            data.push(std::iter::repeat(default).take(self.width() + (size * 2)).collect_vec());
        }

        Grid {data}
    }

    pub fn shrink(&self) -> Grid {
        let mut data = vec![];

        for y in 1..self.height()-1 {
            let mut row = vec![];
            for x in 1..self.width()-1 {
                row.push(self.data[y][x]);
            }
            data.push(row);
        }

        Grid {data}
    }

    pub fn count_lit(&self) -> usize {
        let mut sum = 0;

        for y in 0..self.height() {
            for x in 0..self.width() {
                if self.data[y][x] {
                    sum += 1;
                }
            }
        }

        sum
    }

    pub fn get(&self, x: usize, y: usize) -> bool {
        self.data[y][x]
    }

    pub fn set(&mut self, x: usize, y: usize, value: bool) {
        self.data[y][x] = value;
    }
}