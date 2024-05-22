from flask import Flask, request, jsonify
from Methode_Analytique import Analiz
from FonctionML_KNN import KNN_method
from FonctionML_RF import RandomForest_method
from FonctionML_SVM import SVM_method
import zipfile
app = Flask(__name__)
 
def unzip_file(file_path,extract_to):
    with zipfile.ZipFile(file_path,'r') as zip_ref:
        zip_ref.extractall(extract_to)

@app.route('/analytical', methods=['POST'])
def analytical_route():
    # Call the predict() function to make a prediction
    input_params = request.get_json()
    if input_params is not None:
        sec=input_params.get('sec')
        step=input_params.get('step')
    accuracy, execution_time  = Analiz('data_anonymous',step,sec)
    
    # Return the prediction as JSON
    return jsonify({'accuracy': accuracy, 'execution_time': execution_time})


@app.route('/SVM', methods=['POST'])
def SVM_route():
    input_params = request.get_json()

    if input_params is not None:
        regularisation=input_params.get('regularisation')
        CoefNoyau=input_params.get('CoefNoyau')
        n_plis=input_params.get('n_plis')
        Noyau=input_params.get('Noyau')
        degree= input_params.get('degree')
        coef0=input_params.get('coef0')
        shrinking= input_params.get('shrinking')
        probability=input_params.get('probability')
        tol=input_params.get('tol')
        cache_size=input_params.get('cache_size')
        verbose=input_params.get('verbose')
        max_iter=input_params.get('max_iter')
        decision_function_shape=input_params.get('decision_function_shape')
        break_ties=input_params.get('break_ties')
        random_state=input_params.get('random_state')
        output_dir='Img_Boxplot/New'


        score, conf_matrix, execution_time, details_classement, plot_path  = SVM_method(float(regularisation), float(CoefNoyau), int(n_plis), 
         str(Noyau),'./data_anonymous',int(degree), float(coef0),bool(shrinking),bool(probability),float(tol),float(cache_size),
         bool(verbose), int(max_iter),str(decision_function_shape),bool(break_ties), int(random_state), output_dir  )

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')

        conf_matrix_json = conf_matrix.tolist()
        return jsonify({'score': score, 'matrice_de_confusion': conf_matrix_json, 'temps_execution': execution_time, 'details_classement': details_classement_json, 'path_Img': plot_path})
        
@app.route('/Chemin', methods=['POST'])
def chemin():
    # Get the input parameters from the request
    data = request.get_json()
    chemin=data.get('chemin')
    # Remplacer "\\" par "/"
    chemin = chemin.replace("\\", "/")
    print( chemin)
    unzip_file(chemin,"Uploads/data")
    return chemin

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
        output_dir='Img_Boxplot/New'
        

        
        score, cm, execution_time, details_classement, plot_path = RandomForest_method(int(n_arbres), int(profondeur), int(n_plis), int(n_minimum_split), './data_anonymous', str(criterion), int(min_samples_leaf),
                         float(min_weight_fraction_leaf), str(max_features), int(max_leaf_nodes), float(min_impurity_decrease),
                         bool(bootstrap), bool(oob_score), int(n_jobs), int(random_state), int(verbose), bool(warm_start), str(class_weight), float(ccp_alpha), int(max_samples), output_dir)

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')
        

        cm_json = cm.tolist()
        return jsonify({'score': score, 'matrice_de_confusion': cm_json, 'temps_execution': execution_time, 'details_classement': details_classement_json, 'path_Img': plot_path})



@app.route('/knn', methods=['POST'])
def knn_route():
    input_params = request.get_json()
    if input_params is not None:
        metric = input_params.get('metric')
        n_neighbors= input_params.get('n_neighbors')
        n_plis = input_params.get('n_plis')
        weights = input_params.get('weights')
        algorithm=input_params.get('algorithm') 
        leaf_size=input_params.get('leaf_size')
        p=input_params.get('p')
        n_jobs=input_params.get('n_jobs')
        output_dir='Img_Boxplot/New'


        score, conf_matrix, execution_time, details_classement, plot_path   = KNN_method(str(metric), int(n_neighbors), int(n_plis),
        str(weights), './data_anonymous',str(algorithm),int(leaf_size),float(p),int(n_jobs), output_dir)

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')

        conf_matrix_json = conf_matrix.tolist()
        return jsonify({'score': score, 'matrice_de_confusion': conf_matrix_json, 'temps_execution': execution_time, 'details_classement': details_classement_json, 'path_Img': plot_path})



if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
