from flask import Flask, request, jsonify
from Analiz import Analiz.
from fONCTIONSml import RandomForest_method
from FonctionML_KNN import KNN_method

app = Flask(__name__)

@app.route('/analytical', methods=['POST'])
def analytical_route():
    # Call the predict() function to make a prediction
    accuracy = Analiz('./data_anonymous')
    
    # Return the prediction as JSON
    return jsonify({'accuracy': accuracy})

@app.route('/randomforest', methods=['POST'])
def rf_route():
    input_params = request.get_json()
    if input_params is not None:
        n_arbres = input_params.get('n_arbres')
        profondeur = input_params.get('profondeur')
        n_plis = input_params.get('n_plis')
        n_minimum_split = input_params.get('n_minimum_split')
        

        
        score, cm = RandomForest_method(int(n_arbres), int(profondeur), int(n_plis), int(n_minimum_split), './data_anonymous')

        cm_json = cm.tolist()
        return jsonify({'score': score, 'matrice de confusion':  cm_json})


@app.route('/knn', methods=['POST'])
def knn_route():
    input_params = request.get_json()
    if input_params is not None:
        metric = input_params.get('metric')
        n_neighbor = input_params.get('n_neighbor')
        n_plis = input_params.get('n_plis')
        weights = input_params.get('weights')

        score, conf_matrix = KNN_method(str(metric), int(n_neighbor), int(n_plis), str(weights), './data_anonymous')

        conf_matrix_json = conf_matrix.tolist()
        return jsonify({'score': score, 'matrice de confusion': conf_matrix_json})


@app.route('/SVM', methods=['POST'])
def SVM_route():
    input_params = request.get_json()
    if input_params is not None:
        C = input_params.get('C')
        Kernel = input_params.get('Kernel')
        Gamma = input_params.get('Gamma')
        n_plis = input_params.get('n_plis')

        score, conf_matrix = SVM_method(int(C), float(Gamma), int(n_plis), str(Kernel),  './data_anonymous')

        conf_matrix_json = conf_matrix.tolist()
        return jsonify({'score': score, 'matrice de confusion': conf_matrix_json})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
