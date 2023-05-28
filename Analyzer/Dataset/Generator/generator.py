from random import uniform, getrandbits




class Generator:
    def get_values(self, normal, anomaly, isAnomaly = 0):
        value = self.calculate(normal, anomaly, isAnomaly)
        return value

    def calculate(
        self,
        normal: dict[str, float],
        anomaly: list[dict[str, float]] = {},
        isAnomaly = 0
        ) -> list[float, int]:
        result = 0
        begin = normal.get('begin') 
        end = normal.get('end') 
        
        anomalyRange = getrandbits(1)

        if (isAnomaly == 1 and len(anomaly) > anomalyRange and anomaly[anomalyRange] != None):
            begin = anomaly[anomalyRange].get('begin') 
            end = anomaly[anomalyRange].get('end') 

        result = round(uniform(begin, end), 2)

        return result