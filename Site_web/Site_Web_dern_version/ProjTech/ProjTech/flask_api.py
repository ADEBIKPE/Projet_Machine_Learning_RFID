from flask import Flask, request, jsonify
from Methode_Analytique import Analiz
from FonctionML_RF import RandomForest_method
from FonctionML_KNN import KNN_method
from FonctionML_SVM import SVM_method
app = Flask(__name__)

@app.route('/analytical', methods=['POST'])
def analytical_route():
    # Call the predict() function to make a prediction
    accuracy, execution_time  = Analiz('./data_anonymous')
    
    # Return the prediction as JSON
    return jsonify({'accuracy': accuracy, 'execution_time': execution_time})


@app.route('/SVM', methods=['POST'])
def SVM_route():
    input_params = request.get_json()

    if input_params is not None:
        C = input_params.get('C')
        Kernel = input_params.get('Kernel')
        Gamma = input_params.get('Gamma')
        n_plis = input_params.get('n_plis')

        score, conf_matrix, execution_time, details_classement = SVM_method(float(C), float(Gamma), int(n_plis), str(Kernel),'./data_anonymous')

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')

        conf_matrix_json = conf_matrix.tolist()
        return jsonify({'score': score, 'matrice_de_confusion': conf_matrix_json, 'temps_execution': execution_time, 'details_classement': details_classement_json})
        


@app.route('/randomforest', methods=['POST'])
def rf_route():
    input_params = request.get_json()
    if input_params is not None:
        n_arbres = input_params.get('n_arbres')
        profondeur = input_params.get('profondeur')
        n_plis = input_params.get('n_plis')
        n_minimum_split = input_params.get('n_minimum_split')
        criterion=input_params.get('criterion')
        min_samples_leaf=input_params.get('min_samples_leaf')
        min_weight_fraction_leaf= input_params.get('min_weight_fraction_leaf')
        max_features=input_params.get('max_features')
        max_leaf_nodes= input_params.get('max_leaf_nodes')
        min_impurity_decrease= input_params.get('min_impurity_decrease')
        bootstrap= input_params.get('bootstrap')
        oob_score= input_params.get('oob_score')
        n_jobs= input_params.get('n_jobs')
        random_state=input_params.get('random_state')
        verbose= input_params.get('verbose')
        warm_start= input_params.get('warm_start')
        class_weight= input_params.get('class_weight') 
        ccp_alpha= input_params.get('ccp_alpha')
        max_samples= input_params.get('max_samples')
        

        score, cm, execution_time, details_classement = RandomForest_method(int(n_arbres), int(profondeur), int(n_plis), int(n_minimum_split), './data_anonymous')
        score, cm, execution_time, details_classement = RandomForest_method(int(n_arbres), int(profondeur), int(n_plis), int(n_minimum_split), './data_anonymous', str(criterion), int(min_samples_leaf),
                         float(min_weight_fraction_leaf), str(max_features), int(max_leaf_nodes), float(min_impurity_decrease),
                         bool(bootstrap), bool(oob_score), int(n_jobs), int(random_state), int(verbose), bool(warm_start), str(class_weight), float(ccp_alpha), int(max_samples))

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')
        

        cm_json = cm.tolist()
        return jsonify({'score': score, 'matrice_de_confusion': cm_json, 'temps_execution': execution_time, 'details_classement': details_classement_json})



@app.route('/knn', methods=['POST'])
def knn_route():
    input_params = request.get_json()
    if input_params is not None:
        metric = input_params.get('metric')
        n_neighbor = input_params.get('n_neighbor')
        n_plis = input_params.get('n_plis')
        weights = input_params.get('weights')

        score, conf_matrix, execution_time, details_classement  = KNN_method(str(metric), int(n_neighbor), int(n_plis), str(weights), './data_anonymous')

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')

        conf_matrix_json = conf_matrix.tolist()
        return jsonify({'score': score, 'matrice_de_confusion': conf_matrix_json, 'temps_execution': execution_time, 'details_classement': details_classement_json})



if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
