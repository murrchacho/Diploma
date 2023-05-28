import csv
from generators import temp, tube_temp, tube_pressure, tube_flowmeter
from random import uniform, getrandbits




def get_values():
    #[temp1, temp2, tube_flowmeter1, tube_flowmeter2, tube_pressure1, tube_pressure2, tube_temp1, tube_temp2, isAnomaly]
    result = []
    isAnomaly = False
    functions = [temp, tube_temp, tube_pressure, tube_flowmeter]

    for i in range(3):
        r = []
        for j in range(len(functions)):
            if(i == 0):
                isAnomaly = False
            else:
                isAnomaly = True

            for position in range(2):
                #print(f'Вызываю {functions[j]} {isAnomaly} {position}')
                r.append([*functions[j](isAnomaly, pos=position), isAnomaly])
            result.append([*r])
        result.append('----------------')
    print([*result])
    return [*result]

data = [
        'temp[1]',                
        'temp[2]',                
        'tube_flowmeter[1]',      
        'tube_flowmeter[2]',      
        'tube_pressure[1]',       
        'tube_pressure[2]',       
        'tube_temp[1]',           
        'tube_temp[2]',           
        'anomaly'           
    ]

with open('reactorData.csv', 'w', newline='') as csvfile:
    writer = csv.writer(csvfile)
    writer.writerow(data)
    amount = 3
    for i in range(amount):
        values = get_values()
        writer.writerow(values)
