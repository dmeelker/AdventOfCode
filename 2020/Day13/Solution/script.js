const fs = require('fs');

/**
 * @param {bigint} a 
 * @param {bigint} mod 
 */
function solveMMI(a, mod) {
    const b = a % mod;
    for (let x = 1n; x < mod; x++) {
        if ((b * x) % mod === 1n) {
            return x;
        }
    }
    return 1n;
}

/**
 * @param {{a: bigint, n: bigint}[]} system 
 */
function solveCRT(system) {
    const prod = system.reduce((p, con) => p * con.n, 1n);
    return system.reduce((sm, con) => {
        const p = prod / con.n;
        return sm + (con.a * solveMMI(p, con.n) * p);
    }, 0n) % prod;
}

console.log(fs.readFileSync('input.txt', 'utf-8'));
const congruences = fs.readFileSync('input.txt', 'utf-8')
.split('\n')[1]
.split(',')
.map((id, i) => ({ id, i }))
.filter(eq => eq.id !== 'x')
.map(eq => {
    const n = parseInt(eq.id.trim());
    return {
        n: BigInt(n),
        i: eq.i,
        a: BigInt(n - eq.i)
    };
});

console.log(congruences);

const solution = solveCRT(congruences);
console.log(`Part 2: ${ solution }`);