using System.Collections.Generic;

// Wall types binary storage
// Binary encoding for wall types to be placed in certin locations
public static class WallTypes
{
    // Binary encoding for top wall
    public static HashSet<int> wallTop = new()
    {
        0b1111,
        0b0110,
        0b0011,
        0b0010,
        0b1010,
        0b1100,
        0b1110,
        0b1011,
        0b0111
    };

    // Binary encoding for left wall
    public static HashSet<int> wallSideLeft = new()
    {
        0b0100
    };

    // Binary encoding for right wall
    public static HashSet<int> wallSideRight = new()
    {
        0b0001
    };

    // Binary encoding for bottom wall
    public static HashSet<int> wallBottm = new()
    {
        0b1000
    };

    // Binary encoding for inner corner down left wall
    public static HashSet<int> wallInnerCornerDownLeft = new()
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b01010001,
        0b11010001,
        0b01100001,
        0b11010000,
        0b01110001,
        0b00010001,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001
    };

    // Binray encoding for inner corner down right wall
    public static HashSet<int> wallInnerCornerDownRight = new()
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b01000101,
        0b11000101,
        0b01000011,
        0b10000101,
        0b01000111,
        0b01000100,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010

    };

    // Binary encoding for diagonal corner down left
    public static HashSet<int> wallDiagonalCornerDownLeft = new()
    {
        0b01000000
    };

    // Binary encoding for diagonal corner down right
    public static HashSet<int> wallDiagonalCornerDownRight = new()
    {
        0b00000001
    };

    // Binary encoding for diagonal corner up left
    public static HashSet<int> wallDiagonalCornerUpLeft = new()
    {
        0b00010000,
        0b01010000,
    };

    // Binary encoding for diagonal corner up right
    public static HashSet<int> wallDiagonalCornerUpRight = new()
    {
        0b00000100,
        0b00000101
    };

    // Binary encoding for full wall
    public static HashSet<int> wallFull = new()
    {
        0b1101,
        0b0101,
        0b1101,
        0b1001

    };

    // Binary encoding for full walls to be placed in certin locations
    public static HashSet<int> wallFullEightDirections = new()
    {
        0b00010100,
        0b11100100,
        0b10010011,
        0b01110100,
        0b00010111,
        0b00010110,
        0b00110100,
        0b00010101,
        0b01010100,
        0b00010010,
        0b00100100,
        0b00010011,
        0b01100100,
        0b10010111,
        0b11110100,
        0b10010110,
        0b10110100,
        0b11100101,
        0b11010011,
        0b11110101,
        0b11010111,
        0b11010111,
        0b11110101,
        0b01110101,
        0b01010111,
        0b01100101,
        0b01010011,
        0b01010010,
        0b00100101,
        0b00110101,
        0b01010110,
        0b11010101,
        0b11010100,
        0b10010101,
        0b01110111,
        0b11110111,
        // a
        0b01110110,
        0b00110111,
        0b00110110,
        0b01100111,
        0b01110011,
        0b01100011,
        // a2
        0b11100111,
        0b11110011,
        0b11100011,
    };

    // Binary encoding for 8 directional bottom walls
    public static HashSet<int> wallBottmEightDirections = new()
    {
        0b01000001
    };
}