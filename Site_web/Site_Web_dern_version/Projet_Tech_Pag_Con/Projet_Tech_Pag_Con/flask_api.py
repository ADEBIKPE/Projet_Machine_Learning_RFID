# -*- coding: utf-8 -*-
import os
from flask import Flask, request, jsonify, send_from_directory
from Methode_Analytique import Analiz
from FonctionML_KNN import KNN_method
from FonctionML_RF import RandomForest_method
from FonctionML_SVM import SVM_method
from os.path import relpath
import zipfile
from flask_cors import CORS
import spacy
import json
from sklearn.metrics.pairwise import cosine_similarity
import numpy as np
app = Flask(__name__)
CORS(app) 

def unzip_file(file_path, extract_to):
    with zipfile.ZipFile(file_path, 'r') as zip_ref:
        zip_ref.extractall(extract_to)

@app.route('/analytical', methods=['POST'])
def analytical_route():
    # Call the predict() function to make a prediction
    input_params = request.get_json()
    if input_params is not None:
        sec = input_params.get('sec')
        step = input_params.get('step')
        accuracy, execution_time = Analiz("Uploads/data", int(step), int(sec))
        
        # Return the prediction as JSON
        return jsonify({'accuracy': accuracy, 'execution_time': execution_time})

@app.route('/SVM', methods=['POST'])
def SVM_route():
    input_params = request.get_json()

    if input_params is not None:
        regularisation = input_params.get('regularisation')
        CoefNoyau = input_params.get('CoefNoyau')
        n_plis = input_params.get('n_plis')
        Noyau = input_params.get('Noyau')
        degree = input_params.get('degree')
        coef0 = input_params.get('coef0')
        shrinking = input_params.get('shrinking')
        probability = input_params.get('probability')
        tol = input_params.get('tol')
        cache_size = input_params.get('cache_size')
        verbose = input_params.get('verbose')
        max_iter = input_params.get('max_iter')
        decision_function_shape = input_params.get('decision_function_shape')
        break_ties = input_params.get('break_ties')
        random_state = input_params.get('random_state')
        output_dir = 'wwwroot/images'

        score, conf_matrix, execution_time, details_classement, plot_path = SVM_method(
            float(regularisation), float(CoefNoyau), int(n_plis), str(Noyau), "Uploads/data", int(degree), 
            float(coef0), bool(shrinking), bool(probability), float(tol), float(cache_size), bool(verbose), 
            int(max_iter), str(decision_function_shape), bool(break_ties), int(random_state), output_dir
        )

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')
        conf_matrix_json = conf_matrix.tolist()

        # Convertir le chemin absolu en chemin relatif à partir du dossier wwwroot/images
        plot_url = f'/images/{os.path.basename(plot_path)}'

        return jsonify({
            'score': score, 
            'matrice_de_confusion': conf_matrix_json, 
            'temps_execution': execution_time, 
            'details_classement': details_classement_json, 
            'path_Img': plot_url
        })

@app.route('/Chemin', methods=['POST'])
def chemin():
    # Get the input parameters from the request
    data = request.get_json()
    chemin = data.get('chemin')
    # Remplacer "\\" par "/"
    chemin = chemin.replace("\\", "/")
    print(chemin)
    unzip_file(chemin, "Uploads/data")
    return chemin

@app.route('/randomforest', methods=['POST'])
def rf_route():
    input_params = request.get_json()
    if input_params is not None:
        n_arbres = input_params.get('n_arbres')
        profondeur = input_params.get('profondeur')
        n_plis = input_params.get('n_plis')
        n_minimum_split = input_params.get('n_minimum_split')
        criterion = input_params.get('criterion')
        min_samples_leaf = input_params.get('min_samples_leaf')
        min_weight_fraction_leaf = input_params.get('min_weight_fraction_leaf')
        max_features = input_params.get('max_features')
        max_leaf_nodes = input_params.get('max_leaf_nodes')
        min_impurity_decrease = input_params.get('min_impurity_decrease')
        bootstrap = input_params.get('bootstrap')
        oob_score = input_params.get('oob_score')
        n_jobs = input_params.get('n_jobs')
        random_state = input_params.get('random_state')
        verbose = input_params.get('verbose')
        warm_start = input_params.get('warm_start')
        class_weight = input_params.get('class_weight')
        ccp_alpha = input_params.get('ccp_alpha')
        max_samples = input_params.get('max_samples')
        output_dir = 'wwwroot/images'

        score, cm, execution_time, details_classement, plot_path  = RandomForest_method(
            int(n_arbres), int(profondeur), int(n_plis), int(n_minimum_split), "Uploads/data", str(criterion), 
            int(min_samples_leaf), float(min_weight_fraction_leaf), str(max_features), int(max_leaf_nodes), 
            float(min_impurity_decrease), bool(bootstrap), bool(oob_score), int(n_jobs), int(random_state), 
            int(verbose), bool(warm_start), str(class_weight), float(ccp_alpha), int(max_samples), output_dir
        )

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')
        cm_json = cm.tolist()
        plot_url = '/images/' + os.path.basename(plot_path) if plot_path is not None else 'Aucun mauvais classement trouve.'

        return jsonify({
            'score': score, 
            'matrice_de_confusion': cm_json, 
            'temps_execution': execution_time, 
            'details_classement': details_classement_json, 
            'path_Img': plot_url
        })

@app.route('/knn', methods=['POST'])
def knn_route():
    input_params = request.get_json()
    if input_params is not None:
        metric = input_params.get('metric')
        n_neighbors = input_params.get('n_neighbors')
        n_plis = input_params.get('n_plis')
        weights = input_params.get('weights')
        algorithm = input_params.get('algorithm')
        leaf_size = input_params.get('leaf_size')
        p = input_params.get('p')
        n_jobs = input_params.get('n_jobs')
        output_dir = 'wwwroot/images'

        score, conf_matrix, execution_time, details_classement, plot_path = KNN_method(
            str(metric), int(n_neighbors), int(n_plis), str(weights), "Uploads/data", str(algorithm), 
            int(leaf_size), float(p), int(n_jobs), output_dir
        )

        # Convertir le DataFrame details_classement en un format JSON compatible
        details_classement_json = details_classement.to_dict(orient='records')
        conf_matrix_json = conf_matrix.tolist()

        # Convertir le chemin absolu en chemin relatif à partir du dossier wwwroot/images
        if plot_path is not None:
            plot_url = f'/images/{os.path.basename(plot_path)}'
        else:
            plot_url = None  # ou une autre valeur par défaut, selon vos besoins


        return jsonify({
            'score': score, 
            'matrice_de_confusion': conf_matrix_json, 
            'temps_execution': execution_time, 
            'details_classement': details_classement_json,
            'path_Img': plot_url
        })

# Charger le modèle de SpaCy
nlp = spacy.load('en_core_web_md')

def load_knowledge_base(file_path: str) -> dict:
    try:
        with open(file_path, 'r') as file:
            return json.load(file)
    except (FileNotFoundError, json.JSONDecodeError) as e:
        print(f"Error loading knowledge base: {e}")
        return {"questions": []}

def save_knowledge_base(file_path: str, data: dict):
    try:
        with open(file_path, 'w') as file:
            json.dump(data, file, indent=2)
    except IOError as e:
        print(f"Error saving knowledge base: {e}")

def preprocess_text(text: str) -> str:
    doc = nlp(text)
    return " ".join([token.lemma_ for token in doc if not token.is_stop])

def get_vector(text: str) -> np.ndarray:
    return nlp(text).vector

def find_best_match(user_question: str, questions: list[str]) -> str | None:
    user_vector = get_vector(user_question)
    question_vectors = [get_vector(q) for q in questions]
    similarities = cosine_similarity([user_vector], question_vectors)[0]
    best_index = np.argmax(similarities)
    print(f"User question: '{user_question}' - Best match: '{questions[best_index]}' with similarity: {similarities[best_index]}")  # Debugging print
    return questions[best_index] if similarities[best_index] > 0.6 else None

def get_answer_for_question(question: str, knowledge_base: dict) -> str | None:
    for q in knowledge_base.get("questions", []):
        if q.get("question") == question:
            return q.get("answer")
    return None

knowledge_base = load_knowledge_base('knowledge_base.json')

@app.route('/ask', methods=['POST'])
def ask():
    data = request.json
    user_input = data.get('question', '')
    preprocessed_input = preprocess_text(user_input)
    questions = [preprocess_text(q.get("question", "")) for q in knowledge_base.get("questions", [])]
    best_match = find_best_match(preprocessed_input, questions)

    if best_match:
        original_question = next(q for q in knowledge_base["questions"] if preprocess_text(q["question"]) == best_match)["question"]
        answer = get_answer_for_question(original_question, knowledge_base)
        return jsonify({'answer': answer})
    else:
        return jsonify({'answer': "I don't know the answer. Can you teach me?"})

@app.route('/teach', methods=['POST'])
def teach():
    data = request.json
    new_question = data.get('question', '')
    new_answer = data.get('answer', '')

    knowledge_base.setdefault("questions", []).append({"question": new_question, "answer": new_answer})
    save_knowledge_base('knowledge_base.json', knowledge_base)
    return jsonify({'status': 'success'})


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
