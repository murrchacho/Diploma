import csv
from generators import temp, tube_temp, tube_pressure, tube_flowmeter
from random import uniform, getrandbits




def get_values():
    #[temp1, temp2, tube_flowmeter1, tube_flowmeter2, tube_pressure1, tube_pressure2, tube_temp1, tube_temp2, isAnomaly]
    results = [[],[],[],[]]
    isAnomaly = 0
    functions = [temp, tube_temp, tube_pressure, tube_flowmeter]
    for j in range (len(functions)):
        for i in range(2):
            if (i < 1):
                isAnomaly = 0
            else: 
                isAnomaly = 1
            for position in range(2):
                result = [*functions[j](isAnomaly, pos=position), isAnomaly]
                for value in result[:len(result)-1]:
                    results[j].append(value)
    return results

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
    amount = 100000
    for i in range(amount):
        values = get_values()
        for j in range(6):
            results = []
            for value in values:
                results.append(value[j])
                results.append(value[j+1])

            if (j < 3):
                results.append(0)
            else:
                results.append(1)
            writer.writerow(results)
