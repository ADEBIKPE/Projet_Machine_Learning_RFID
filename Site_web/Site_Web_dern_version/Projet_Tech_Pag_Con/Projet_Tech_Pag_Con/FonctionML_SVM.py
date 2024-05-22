def SVM_method(regularisation, CoefNoyau, n_plis, Noyau, pathfile, degree, 
               coef0, shrinking, probability, tol, cache_size, verbose, max_iter,
               decision_function_shape, break_ties, random_state, output_dir):
    # Charger les données
    from preprocessing import preprocessing
    from sklearn.model_selection import cross_val_score, cross_val_predict
    from sklearn.svm import SVC
    from sklearn.metrics import confusion_matrix
    from sklearn.preprocessing import LabelEncoder
    from selectfeatures import Xcols_func
    from sklearn.preprocessing import StandardScaler
    import time
    import pandas as pd
    import matplotlib.pyplot as plt
    import seaborn as sns
    import os

    start_time = time.time()
    X = Xcols_func('rssi & rc only', preprocessing(pathfile).columns)
    ds = preprocessing(pathfile)
    x = ds.loc[:, X]
    y = ds['in_or_out']
    
    # Créer un encodeur LabelEncoder
    label_encoder = LabelEncoder()

    # Encoder la variable cible
    y_encoded = label_encoder.fit_transform(y)
    
    # Normaliser les données
    scaler = StandardScaler()
    x_scaled = scaler.fit_transform(x)
    
    # Créer le modèle avec tous les hyperparamètres spécifiés
    svm_model = SVC(C=regularisation, kernel=Noyau, degree=degree, gamma=CoefNoyau, coef0=coef0, shrinking=shrinking,
                     probability=probability, tol=tol, cache_size=cache_size, verbose=verbose,
                       max_iter=max_iter, decision_function_shape=decision_function_shape, break_ties=break_ties,
                         random_state=random_state)
    
    # Effectuer une validation croisée
    scores = cross_val_score(svm_model, x_scaled, y_encoded, cv=n_plis)
    
    # Afficher les scores de validation croisée
    print("Scores de validation croisée:", scores)
    print("Score moyen de validation croisée:", scores.mean())
    score = scores.mean()
    
    # Entraîner le modèle sur toutes les données
    svm_model.fit(x_scaled, y_encoded)
    
    # Prédiction avec la validation croisée
    y_pred = cross_val_predict(svm_model, x_scaled, y_encoded, cv=n_plis)
    
    # Identifier les faux positifs et les faux négatifs
    faux_inside = [i for i in range(len(y)) if (y_pred[i] == 0 and y_encoded[i] != y_pred[i])]
    faux_outside = [i for i in range(len(y)) if (y_pred[i] == 1 and y_encoded[i] != y_pred[i])]
    
    # Nom de la colonne que vous souhaitez extraire
    column_names = ['Epc', 'reflist_run_id', 'refListId_actual']
    false_inside_columns = {}
    false_outside_columns = {}
    for colonne in column_names:
        # Obtenir l'indice de la colonne à partir de son nom
        column_index = pd.Index(ds.columns).get_loc(colonne)
        if colonne == 'Epc':
            false_inside_columns[colonne] = ds.iloc[faux_inside, column_index]
            false_outside_columns[colonne] = ds.iloc[faux_outside, column_index]
        elif colonne == 'reflist_run_id':
            false_inside_columns[colonne] = [elem.split('_')[0] for elem in ds.iloc[faux_inside, column_index]]
            false_outside_columns[colonne] = [elem.split('_')[0] for elem in ds.iloc[faux_outside, column_index]]
        else:
            false_inside_columns[colonne] = ds.iloc[faux_inside, column_index]
            false_outside_columns[colonne] = ds.iloc[faux_outside, column_index]

    details_classement = pd.DataFrame({
        'Tags': false_inside_columns['Epc'],
        'Classé dans la boîte': false_inside_columns['reflist_run_id'],
        'Devrait être classé dans la boite': false_inside_columns['refListId_actual']
    })

    details_classement.drop_duplicates(inplace=True)
    
    # Calcul de la matrice de confusion
    conf_matrix = confusion_matrix(y_encoded, y_pred)

    end_time = time.time()

    # Calcul du temps d'exécution
    execution_time = end_time - start_time
    
    # Extraire les nombres des chaînes de caractères pour les EPC correctement et incorrectement classés
    correct_epcs_numbers = details_classement['Tags'].str.extract(r'(\d+)', expand=False).astype(int)
    incorrect_epcs_numbers = details_classement['Classé dans la boîte'].str.extract(r'(\d+)', expand=False).astype(int)
    
    # Créer une boîte à moustaches avec Seaborn
    epc_data = pd.DataFrame({
        'EPC Numbers': pd.concat([correct_epcs_numbers, incorrect_epcs_numbers]).reset_index(drop=True),
        'Classification': ['Correct'] * len(correct_epcs_numbers) + ['Incorrect'] * len(incorrect_epcs_numbers)
    })
    
    plt.figure(figsize=(10, 6))
    sns.boxplot(x='Classification', y='EPC Numbers', hue='Classification', data=epc_data, palette='Set3', dodge=False)
    plt.title('Comparaison des EPCs correctement et incorrectement classés')
    plt.ylabel('Nombre d\'EPCs')
    plt.grid(True)
    plt.tight_layout()

    # Vérifier et créer le répertoire de sortie s'il n'existe pas
    output_dir_path = os.path.join(os.getcwd(), output_dir)
    os.makedirs(output_dir_path, exist_ok=True)

    # Enregistrer le graphique dans le répertoire de sortie
    plot_path = os.path.join(output_dir_path, 'boxplot_EPC_comparison_seaborn_SVM.png')
    plt.savefig(plot_path)
    plt.close()
    
    return score, conf_matrix, execution_time, details_classement, plot_path

# Appel de la fonction SVM_method et récupération des résultats
'''
score, conf_matrix, execution_time, details_classement, plot_path = SVM_method(
    regularisation=1.0,
    CoefNoyau=10,
    n_plis=5,
    Noyau='rbf',
    pathfile='./data_anonymous',
    degree=1,
    coef0=0.1,
    shrinking=True,
    probability=False,
    tol=0.002,
    cache_size=200,
    verbose=False,
    max_iter=1000,
    decision_function_shape='ovr',
    break_ties=False,
    random_state=40,
    output_dir='Img_Boxplot/New'
)
'''
