/*
--- Day 16: Packet Decoder ---
As you leave the cave and reach open waters, you receive a transmission from the Elves back on the ship.

The transmission was sent using the Buoyancy Interchange Transmission System (BITS), a method of packing numeric expressions into a binary sequence. Your submarine's computer has saved the transmission in hexadecimal (your puzzle input).

The first step of decoding the message is to convert the hexadecimal representation into binary. Each character of hexadecimal corresponds to four bits of binary data:

0 = 0000
1 = 0001
2 = 0010
3 = 0011
4 = 0100
5 = 0101
6 = 0110
7 = 0111
8 = 1000
9 = 1001
A = 1010
B = 1011
C = 1100
D = 1101
E = 1110
F = 1111
The BITS transmission contains a single packet at its outermost layer which itself contains many other packets. The hexadecimal representation of this packet might encode a few extra 0 bits at the end; these are not part of the transmission and should be ignored.

Every packet begins with a standard header: the first three bits encode the packet version, and the next three bits encode the packet type ID. These two values are numbers; all numbers encoded in any packet are represented as binary with the most significant bit first. For example, a version encoded as the binary sequence 100 represents the number 4.

Packets with type ID 4 represent a literal value. Literal value packets encode a single binary number. To do this, the binary number is padded with leading zeroes until its length is a multiple of four bits, and then it is broken into groups of four bits. Each group is prefixed by a 1 bit except the last group, which is prefixed by a 0 bit. These groups of five bits immediately follow the packet header. For example, the hexadecimal string D2FE28 becomes:

110100101111111000101000
VVVTTTAAAAABBBBBCCCCC
Below each bit is a label indicating its purpose:

The three bits labeled V (110) are the packet version, 6.
The three bits labeled T (100) are the packet type ID, 4, which means the packet is a literal value.
The five bits labeled A (10111) start with a 1 (not the last group, keep reading) and contain the first four bits of the number, 0111.
The five bits labeled B (11110) start with a 1 (not the last group, keep reading) and contain four more bits of the number, 1110.
The five bits labeled C (00101) start with a 0 (last group, end of packet) and contain the last four bits of the number, 0101.
The three unlabeled 0 bits at the end are extra due to the hexadecimal representation and should be ignored.
So, this packet represents a literal value with binary representation 011111100101, which is 2021 in decimal.

Every other type of packet (any packet with a type ID other than 4) represent an operator that performs some calculation on one or more sub-packets contained within. Right now, the specific operations aren't important; focus on parsing the hierarchy of sub-packets.

An operator packet contains one or more packets. To indicate which subsequent binary data represents its sub-packets, an operator packet can use one of two modes indicated by the bit immediately after the packet header; this is called the length type ID:

If the length type ID is 0, then the next 15 bits are a number that represents the total length in bits of the sub-packets contained by this packet.
If the length type ID is 1, then the next 11 bits are a number that represents the number of sub-packets immediately contained by this packet.
Finally, after the length type ID bit and the 15-bit or 11-bit field, the sub-packets appear.

For example, here is an operator packet (hexadecimal string 38006F45291200) with length type ID 0 that contains two sub-packets:

00111000000000000110111101000101001010010001001000000000
VVVTTTILLLLLLLLLLLLLLLAAAAAAAAAAABBBBBBBBBBBBBBBB
The three bits labeled V (001) are the packet version, 1.
The three bits labeled T (110) are the packet type ID, 6, which means the packet is an operator.
The bit labeled I (0) is the length type ID, which indicates that the length is a 15-bit number representing the number of bits in the sub-packets.
The 15 bits labeled L (000000000011011) contain the length of the sub-packets in bits, 27.
The 11 bits labeled A contain the first sub-packet, a literal value representing the number 10.
The 16 bits labeled B contain the second sub-packet, a literal value representing the number 20.
After reading 11 and 16 bits of sub-packet data, the total length indicated in L (27) is reached, and so parsing of this packet stops.

As another example, here is an operator packet (hexadecimal string EE00D40C823060) with length type ID 1 that contains three sub-packets:

11101110000000001101010000001100100000100011000001100000
VVVTTTILLLLLLLLLLLAAAAAAAAAAABBBBBBBBBBBCCCCCCCCCCC
The three bits labeled V (111) are the packet version, 7.
The three bits labeled T (011) are the packet type ID, 3, which means the packet is an operator.
The bit labeled I (1) is the length type ID, which indicates that the length is a 11-bit number representing the number of sub-packets.
The 11 bits labeled L (00000000011) contain the number of sub-packets, 3.
The 11 bits labeled A contain the first sub-packet, a literal value representing the number 1.
The 11 bits labeled B contain the second sub-packet, a literal value representing the number 2.
The 11 bits labeled C contain the third sub-packet, a literal value representing the number 3.
After reading 3 complete sub-packets, the number of sub-packets indicated in L (3) is reached, and so parsing of this packet stops.

For now, parse the hierarchy of the packets throughout the transmission and add up all of the version numbers.

Here are a few more examples of hexadecimal-encoded transmissions:

8A004A801A8002F478 represents an operator packet (version 4) which contains an operator packet (version 1) which contains an operator packet (version 5) which contains a literal value (version 6); this packet has a version sum of 16.
620080001611562C8802118E34 represents an operator packet (version 3) which contains two sub-packets; each sub-packet is an operator packet that contains two literal values. This packet has a version sum of 12.
C0015000016115A2E0802F182340 has the same structure as the previous example, but the outermost packet uses a different length type ID. This packet has a version sum of 23.
A0016C880162017C3686B18A3D4780 is an operator packet that contains an operator packet that contains an operator packet that contains five literal values; it has a version sum of 31.
Decode the structure of your hexadecimal-encoded BITS transmission; what do you get if you add up the version numbers in all packets?
*/

using System.Diagnostics;

List<bool> ParseHexString(string input)
{
    var result = new List<bool>();
    foreach (var c in input)
    {
        var v = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
        var b = Convert.ToString(v, 2);
        b = b.PadLeft(4, '0');
        result.AddRange(b.Select(x => x == '1'));
    }
    return result;
}

long BoolsToLong(IEnumerable<bool> bools)
{
    long result = 0;
    foreach (var b in bools)
    {
        result <<= 1;
        result += b ? 1 : 0;
    }
    return result;
}

IEnumerable<T> Dequeue<T>(Queue<T> queue, int count)
{
    for (var i = 0; i < count; i++)
    {
        yield return queue.Dequeue();
    }
}

Packet ParsePacketAsQueue(Queue<bool> queue)
{
    var originalQueueLength = queue.Count;
    
    var result = new Packet();
    
    result.Version = (int)BoolsToLong(Dequeue(queue, 3));
    
    var typeCode = BoolsToLong(Dequeue(queue, 3));
    result.Type = (PacketType)typeCode;
    
    if (result.Type == PacketType.Literal)
    {
        result.Value = 0;
        bool hasNext;
        do
        {
            hasNext = queue.Dequeue();
            result.Value <<= 4;
            result.Value += BoolsToLong(Dequeue(queue, 4));
        } while (hasNext);
    }
    else
    {
        result.LengthType = queue.Dequeue() ? LengthType.SubPackets : LengthType.Bits;

        result.SubPackets = new List<Packet>();

        if (result.LengthType == LengthType.Bits)
        {
            result.Length = BoolsToLong(Dequeue(queue, 15));

            var subPacketBits = 0;

            while (subPacketBits < result.Length)
            {
                var subPacket = ParsePacketAsQueue(queue);
                result.SubPackets.Add(subPacket);
                subPacketBits += subPacket.TotalBits;
            }
        }
        else if (result.LengthType == LengthType.SubPackets)
        {
            result.Length = BoolsToLong(Dequeue(queue, 11));

            for (var i = 0; i < result.Length; i++)
            {
                var subPacket = ParsePacketAsQueue(queue);
                result.SubPackets.Add(subPacket);
            }
        }
    }

    result.TotalBits = originalQueueLength - queue.Count;
    return result;
}

int GetVersionSum(Packet packet)
{
    var subPacketSum = packet.SubPackets == null ? 0 : packet.SubPackets.Sum(GetVersionSum);
    return packet.Version + subPacketSum;
}

Packet ParseAsQueue(string input)
{
    return ParsePacketAsQueue(new Queue<bool>(ParseHexString(input)));
}

// Test literal
var packet = ParseAsQueue("D2FE28");
Debug.Assert(packet.Version == 6);
Debug.Assert(packet.Type == PacketType.Literal);
Debug.Assert(packet.Value == 2021, "Expected 2021, got " + packet.Value);
Debug.Assert(packet.TotalBits == 21, "Expected 21, got " + packet.TotalBits);


// Test operator
var packet2 = ParseAsQueue("38006F45291200");
Debug.Assert(packet2.Version == 1);
Debug.Assert(packet2.SubPackets.Count == 2, "Expected 2, got " + packet2.SubPackets.Count);
Debug.Assert(packet2.SubPackets[0].Version == 6, "Expected 6, got " + packet2.SubPackets[0].Version);
Debug.Assert(packet2.SubPackets[0].Type == PacketType.Literal, "Expected Literal, got " + packet2.SubPackets[0].Type);
Debug.Assert(packet2.SubPackets[0].Value == 10, "Expected 10, got " + packet2.SubPackets[0].Value);
Debug.Assert(packet2.SubPackets[0].TotalBits == 11, "Expected 11, got " + packet2.SubPackets[0].TotalBits);

Debug.Assert(packet2.SubPackets[1].Version == 2, "Expected 2, got " + packet2.SubPackets[1].Version);
Debug.Assert(packet2.SubPackets[1].Type == PacketType.Literal, "Expected Literal, got " + packet2.SubPackets[1].Type);
Debug.Assert(packet2.SubPackets[1].Value == 20, "Expected 20, got " + packet2.SubPackets[1].Value);

// Test operator with subpackets
var packet3 = ParseAsQueue("EE00D40C823060");
Debug.Assert(packet3.Version == 7);
Debug.Assert(packet3.SubPackets.Count == 3, "Expected 3, got " + packet3.SubPackets.Count);
Debug.Assert(packet3.SubPackets[0].Version == 2, "Expected 2, got " + packet3.SubPackets[0].Version);
Debug.Assert(packet3.SubPackets[0].Type == PacketType.Literal, "Expected Literal, got " + packet3.SubPackets[0].Type);
Debug.Assert(packet3.SubPackets[0].Value == 1, "Expected 1, got " + packet3.SubPackets[0].Value);
Debug.Assert(packet3.SubPackets[0].TotalBits == 11, "Expected 11, got " + packet3.SubPackets[0].TotalBits);

Debug.Assert(packet3.SubPackets[1].Version == 4, "Expected 4, got " + packet3.SubPackets[1].Version);
Debug.Assert(packet3.SubPackets[1].Type == PacketType.Literal, "Expected Literal, got " + packet3.SubPackets[1].Type);
Debug.Assert(packet3.SubPackets[1].Value == 2, "Expected 2, got " + packet3.SubPackets[1].Value);
Debug.Assert(packet3.SubPackets[1].TotalBits == 11, "Expected 11, got " + packet3.SubPackets[1].TotalBits);

Debug.Assert(packet3.SubPackets[2].Version == 1, "Expected 1, got " + packet3.SubPackets[2].Version);
Debug.Assert(packet3.SubPackets[2].Type == PacketType.Literal, "Expected Literal, got " + packet3.SubPackets[2].Type);
Debug.Assert(packet3.SubPackets[2].Value == 3, "Expected 3, got " + packet3.SubPackets[2].Value);
Debug.Assert(packet3.SubPackets[2].TotalBits == 11, "Expected 11, got " + packet3.SubPackets[2].TotalBits);

var packet4 = ParseAsQueue("8A004A801A8002F478");
Debug.Assert(packet4.Version == 4);

Debug.Assert(packet4.SubPackets.Count == 1);
Debug.Assert(packet4.SubPackets[0].Version == 1);
Debug.Assert(packet4.SubPackets[0].SubPackets.Count == 1);

Debug.Assert(packet4.SubPackets[0].SubPackets[0].Version == 5, "Expected 5, got " + packet4.SubPackets[0].SubPackets[0].Version);
Debug.Assert(packet4.SubPackets[0].SubPackets[0].SubPackets.Count == 1);

Debug.Assert(packet4.SubPackets[0].SubPackets[0].SubPackets[0].Version == 6);
Debug.Assert(packet4.SubPackets[0].SubPackets[0].SubPackets[0].Type == PacketType.Literal);

Debug.Assert(GetVersionSum(packet4) == 16);

Debug.Assert(GetVersionSum(ParseAsQueue("620080001611562C8802118E34")) == 12);
Debug.Assert(GetVersionSum(ParseAsQueue("C0015000016115A2E0802F182340")) == 23);
Debug.Assert(GetVersionSum(ParseAsQueue("A0016C880162017C3686B18A3D4780")) == 31);

var input = File.ReadAllText("input.txt");
var part1Packet = ParseAsQueue(input);
Console.WriteLine("Part 1: " + GetVersionSum(part1Packet));








/*
--- Part Two ---
Now that you have the structure of your transmission decoded, you can calculate the value of the expression it represents.

Literal values (type ID 4) represent a single number as described above. The remaining type IDs are more interesting:

Packets with type ID 0 are sum packets - their value is the sum of the values of their sub-packets. If they only have a single sub-packet, their value is the value of the sub-packet.
Packets with type ID 1 are product packets - their value is the result of multiplying together the values of their sub-packets. If they only have a single sub-packet, their value is the value of the sub-packet.
Packets with type ID 2 are minimum packets - their value is the minimum of the values of their sub-packets.
Packets with type ID 3 are maximum packets - their value is the maximum of the values of their sub-packets.
Packets with type ID 5 are greater than packets - their value is 1 if the value of the first sub-packet is greater than the value of the second sub-packet; otherwise, their value is 0. These packets always have exactly two sub-packets.
Packets with type ID 6 are less than packets - their value is 1 if the value of the first sub-packet is less than the value of the second sub-packet; otherwise, their value is 0. These packets always have exactly two sub-packets.
Packets with type ID 7 are equal to packets - their value is 1 if the value of the first sub-packet is equal to the value of the second sub-packet; otherwise, their value is 0. These packets always have exactly two sub-packets.
Using these rules, you can now work out the value of the outermost packet in your BITS transmission.

For example:

C200B40A82 finds the sum of 1 and 2, resulting in the value 3.
04005AC33890 finds the product of 6 and 9, resulting in the value 54.
880086C3E88112 finds the minimum of 7, 8, and 9, resulting in the value 7.
CE00C43D881120 finds the maximum of 7, 8, and 9, resulting in the value 9.
D8005AC2A8F0 produces 1, because 5 is less than 15.
F600BC2D8F produces 0, because 5 is not greater than 15.
9C005AC2F8F0 produces 0, because 5 is not equal to 15.
9C0141080250320F1802104A08 produces 1, because 1 + 3 = 2 * 2.
What do you get if you evaluate the expression represented by your hexadecimal-encoded BITS transmission?
*/



long CalculateValue(Packet packet)
{
    if (packet.Type == PacketType.Literal)
        return packet.Value;
    
    if (packet.Type == PacketType.Sum)
        return packet.SubPackets.Sum(CalculateValue);
    
    if (packet.Type == PacketType.Product)
        return packet.SubPackets.Aggregate(1L, (acc, p) => acc * CalculateValue(p));
    
    if (packet.Type == PacketType.Minimum)
        return packet.SubPackets.Min(CalculateValue);
    
    if (packet.Type == PacketType.Maximum)
        return packet.SubPackets.Max(CalculateValue);
    
    if (packet.Type == PacketType.GreaterThan)
        return CalculateValue(packet.SubPackets[0]) > CalculateValue(packet.SubPackets[1]) ? 1 : 0;
    
    if (packet.Type == PacketType.LessThan)
        return CalculateValue(packet.SubPackets[0]) < CalculateValue(packet.SubPackets[1]) ? 1 : 0;
    
    if (packet.Type == PacketType.EqualTo)
        return CalculateValue(packet.SubPackets[0]) == CalculateValue(packet.SubPackets[1]) ? 1 : 0;
    
    throw new Exception("Unknown packet type: " + packet.Type);
}

var packet6 = ParseAsQueue("C200B40A82");
Debug.Assert(CalculateValue(packet6) == 3);

var packet7 = ParseAsQueue("04005AC33890");
Debug.Assert(CalculateValue(packet7) == 54);

var packet8 = ParseAsQueue("880086C3E88112");
Debug.Assert(CalculateValue(packet8) == 7);

var packet9 = ParseAsQueue("CE00C43D881120");
Debug.Assert(CalculateValue(packet9) == 9);

var packet10 = ParseAsQueue("D8005AC2A8F0");
Debug.Assert(CalculateValue(packet10) == 1);

var packet11 = ParseAsQueue("F600BC2D8F");
Debug.Assert(CalculateValue(packet11) == 0);

var packet12 = ParseAsQueue("9C005AC2F8F0");
Debug.Assert(CalculateValue(packet12) == 0);

var packet13 = ParseAsQueue("9C0141080250320F1802104A08");
Debug.Assert(CalculateValue(packet13) == 1);

Console.WriteLine("Part 2: " + CalculateValue(part1Packet));


public enum PacketType
{
    Sum = 0,
    Product = 1,
    Minimum = 2,
    Maximum = 3,
    Literal = 4,
    GreaterThan = 5,
    LessThan = 6,
    EqualTo = 7
}


public enum LengthType
{
    Bits = 0,
    SubPackets = 1
}

public class Packet
{
    public PacketType Type { get; set; }
    public int Version { get; set; }
    public LengthType LengthType { get; set; }
    public long Length { get; set; }
    public List<Packet> SubPackets { get; set; }
    public long Value { get; set; }
    public int TotalBits { get; set; }
}

