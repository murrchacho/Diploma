import pandas as pd
import keras as k
import numpy as np

test = True
path = 'weights.h5'
data_frame = pd.read_csv("../Generator/reactorData.csv")

if (test):
    data_frame = pd.read_csv("../Generator/reactorTest.csv")

print(data_frame)

input_values = ["temp[1]", "temp[2]", "tube_flowmeter[1]", "tube_flowmeter[2]", "tube_pressure[1]", "tube_pressure[2]", "tube_temp[1]", "tube_temp[2]"]
output_value = ["anomaly"]

encoders = {"temp[1]": lambda value: [value],
            "temp[2]": lambda value: [value],
            "tube_flowmeter[1]": lambda value: [value],
            "tube_flowmeter[2]": lambda value: [value],
            "tube_pressure[1]": lambda value: [value],
            "tube_pressure[2]": lambda value: [value],
            "tube_temp[1]": lambda value: [value],
            "tube_temp[2]": lambda value: [value],
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
model.add(k.layers.Dense(units=5, activation='relu'))
model.add(k.layers.Dense(units=1, activation='sigmoid'))
model.compile(loss='mse', optimizer='sgd', metrics=['accuracy'])

if (test == False):
    fit_results = model.fit(x=train_x, y=train_y, epochs=100, validation_split=0.2)
    model.save(path)
    print('Обучение завершено!')


if (test):
    model = k.models.load_model(path)
    predicted_test = model.predict(train_x)
    real_data = data_frame
    real_data['ANOMALY DETECTED'] = predicted_test
    real_data.to_csv('results.csv')

    with pd.option_context('display.max_rows', None, 'display.max_columns', None):  # more options can be specified also
        print(real_data)