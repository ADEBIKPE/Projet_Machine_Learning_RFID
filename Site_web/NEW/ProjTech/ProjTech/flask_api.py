from flask import Flask, request, jsonify
from Analiz import Analiz
from fONCTIONSml import RandomForest_method

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

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
