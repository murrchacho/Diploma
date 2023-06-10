import pandas as pd
import keras as k
import numpy as np

test = True
path = 'weights.h5'
data_frame = pd.read_csv("../Generator/reactorData.csv")

if (test):
    data_frame = pd.read_csv("../Generator/reactorTest.csv")
#data_frame = (data_frame - data_frame.min())/(data_frame.max() - data_frame.min())


input_values = ["temp[1]", "temp[2]", "tube_flowmeter[1]", "tube_flowmeter[2]", "tube_pressure[1]", "tube_pressure[2]", "tube_temp[1]", "tube_temp[2]"]
output_value = ["anomaly"]

temp1_min = data_frame['temp[1]'].min()
temp1_max = data_frame['temp[1]'].max()

temp2_min = data_frame['temp[2]'].min()
temp2_max = data_frame['temp[2]'].max()

tube1_flow_min = data_frame['tube_flowmeter[1]'].min()
tube1_flow_max = data_frame['tube_flowmeter[1]'].max()

tube2_flow_min = data_frame['tube_flowmeter[2]'].min()
tube2_flow_max = data_frame['tube_flowmeter[2]'].max()

tube1_pres_min = data_frame['tube_pressure[1]'].min()
tube1_pres_max = data_frame['tube_pressure[1]'].max()

tube2_pres_min = data_frame['tube_pressure[2]'].min()
tube2_pres_max =  data_frame['tube_pressure[2]'].max()

tube1_temp_min =  data_frame['tube_temp[1]'].min()
tube1_temp_max = data_frame['tube_temp[1]'].max()

tube2_temp_min = data_frame['tube_temp[2]'].min()
tube2_temp_max = data_frame['tube_temp[2]'].max()

encoders = {"temp[1]": lambda value: [round((value - temp1_min)/(temp1_max -  temp1_min),2)],
            "temp[2]": lambda value: [round((value - temp2_min)/(temp2_max -  temp2_min),2)],
            "tube_flowmeter[1]": lambda value: [round((value - tube1_flow_min)/(tube1_flow_max - tube1_flow_min),2)],
            "tube_flowmeter[2]": lambda value: [round((value - tube2_flow_min)/(tube2_flow_max  - tube2_flow_min),2)],
            "tube_pressure[1]": lambda value: [round((value - tube1_pres_min )/(tube1_pres_max  -  tube1_pres_min ),2)],
            "tube_pressure[2]": lambda value: [round((value - tube2_pres_min )/(tube2_pres_max  -  tube2_pres_min ),2)],
            "tube_temp[1]": lambda value: [round((value - tube1_temp_min)/(tube1_temp_max -  tube1_temp_min),2)],
            "tube_temp[2]": lambda value: [round((value - tube2_temp_min)/(tube2_temp_max -  tube2_temp_min),2)],
            "anomaly": lambda value: [value]}

def dataframe_to_dict(df):
    result = dict()
    for column in df.columns:
        values = data_frame[column].values
        result[column] = values
    return result

def make_supervised(df):
    raw_input_data = data_frame[input_values]
    raw_output_data = data_frame[output_value]

    return {"inputs": dataframe_to_dict(raw_input_data),
            "outputs": dataframe_to_dict(raw_output_data)}

def encode(data):
    vectors = []
    for data_name, data_values in data.items():
        encoded = list(map(encoders[data_name], data_values)) 
        vectors.append(encoded)
    formatted = []
    for vector_raw in list(zip(*vectors)):
        vector = []
        for element in vector_raw:
            for e in element:
                vector.append(e)
        formatted.append(vector)
    
    return formatted


supervised = make_supervised(data_frame)
encoded_inputs = np.array(encode(supervised["inputs"]))
encoded_outputs = np.array(encode(supervised["outputs"]))

train_x = encoded_inputs
train_y = encoded_outputs

model = k.Sequential()
model.add(k.layers.Dense(units=9, activation='relu'))
model.add(k.layers.Dense(units=1, activation='sigmoid'))
model.compile(loss='mse', optimizer='sgd', metrics=['accuracy'])

if (test == False):
    fit_results = model.fit(x=train_x, y=train_y, epochs=10, validation_split=0.2, shuffle=True)
    model.save(path)
    print('Обучение завершено!')


if (test):
    model = k.models.load_model(path)
    predicted_test = model.predict(train_x)
    real_data = data_frame
    real_data['ANOMALY DETECTED'] = np.round(predicted_test,0)
    real_data.to_csv('results.csv')

    #with pd.option_context():  # more options can be specified also
    print(real_data)