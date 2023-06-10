from generator import Generator
from random import uniform, getrandbits

generator = Generator();


def inaccuracy(value):
    return value + round(uniform(-2, 5))


def temp(isAnomaly, pos):
    temp1 = generator.get_values(normal={'begin': 50, 'end': 70}, anomaly=[{'begin': 0, 'end': 49}, {'begin': 75, 'end': 100}], isAnomaly=isAnomaly and pos==0)
    temp2 = generator.get_values(normal={'begin': inaccuracy(temp1), 'end': inaccuracy(temp1)}, anomaly=[{'begin': 0, 'end': 49}, {'begin': 75, 'end': 100}], isAnomaly=isAnomaly and pos==1)
    return [temp1, temp2]


def tube_flowmeter(isAnomaly, pos):
    tube_flowmeter1 = generator.get_values(normal={'begin': 50, 'end': 70}, anomaly=[{'begin': 0, 'end': 49}, {'begin': 75, 'end': 100}], isAnomaly=isAnomaly and pos==0)
    tube_flowmeter2 = generator.get_values(normal={'begin': tube_flowmeter1, 'end': tube_flowmeter1}, anomaly=[{'begin': 0, 'end': tube_flowmeter1}, {'begin': tube_flowmeter1, 'end': 100}], isAnomaly=isAnomaly and pos==1)

    return [tube_flowmeter1, tube_flowmeter2]


def tube_pressure(isAnomaly, pos):
    tube_pressure1 = generator.get_values(normal={'begin': 10, 'end': 20}, anomaly=[{'begin': 0, 'end': 10}, {'begin': 20, 'end': 100}], isAnomaly=isAnomaly and pos==0)
    tube_pressure_inaccuracy = inaccuracy(tube_pressure1)
    tube_pressure2 = generator.get_values(normal={'begin': tube_pressure_inaccuracy, 'end': tube_pressure_inaccuracy}, anomaly=[{'begin': 0, 'end': tube_pressure_inaccuracy}, {'begin': tube_pressure_inaccuracy, 'end': 100}], isAnomaly=isAnomaly and pos==1)

    return [tube_pressure1, tube_pressure2]

def tube_temp(isAnomaly, pos):
    tube_temp1 = generator.get_values(normal={'begin': 0, 'end': 30}, anomaly=[{'begin': 30, 'end': 60}], isAnomaly=isAnomaly and pos==0)
    tube_temp_inaccuracy = inaccuracy(tube_temp1)
    tube_temp2 = generator.get_values(normal={'begin': tube_temp_inaccuracy, 'end': tube_temp_inaccuracy}, anomaly=[{'begin': 0, 'end': tube_temp_inaccuracy}, {'begin': tube_temp_inaccuracy, 'end': 100}], isAnomaly=isAnomaly and pos==1)
    
    return [tube_temp1, tube_temp2]