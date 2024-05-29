def KNN_method(metric, n_neighbors, n_plis, weights, pathfile, algorithm, leaf_size, p, n_jobs, output_dir):
    # Charger les données
    import time
    import matplotlib
    matplotlib.use('Agg')  # Utiliser la backend Agg de Matplotlib pour éviter les problèmes de thread
    import matplotlib.pyplot as plt
    import seaborn as sns
    from sklearn.model_selection import cross_val_score, cross_val_predict
    from sklearn.neighbors import KNeighborsClassifier
    from sklearn.preprocessing import StandardScaler, LabelEncoder
    from sklearn.metrics import confusion_matrix
    from preprocessing import preprocessing
    from selectfeatures import Xcols_func
    import pandas as pd
    import os
    
    start_time = time.time()
    X = Xcols_func('rssi & rc only', preprocessing(pathfile).columns)
    ds = preprocessing(pathfile)
    x = ds.loc[:, X]
    y = ds['in_or_out']
    
    # Créer un encodeur de labels
    label_encoder = LabelEncoder()
    # Création d'un objet StandardScaler
    scaler = StandardScaler()
    # Normalisation des caractéristiques
    X_normalized = scaler.fit_transform(x)
    # Appliquer l'encodage aux valeurs de la colonne 'in_or_out'
    y = label_encoder.fit_transform(y)
    
    # Création du classificateur KNN avec tous les hyperparamètres spécifiés
    knn = KNeighborsClassifier(metric=metric, n_neighbors=n_neighbors, weights=weights, algorithm=algorithm, 
                               leaf_size=leaf_size, p=p, n_jobs=n_jobs)
    
    # Validation croisée avec 5 folds et une métrique de score spécifiée
    cv_scores = cross_val_score(knn, X_normalized, y, cv=n_plis)
    score = cv_scores.mean()
    
    # Faire des prédictions avec validation croisée
    y_pred = cross_val_predict(knn, X_normalized, y, cv=n_plis)
    faux_inside = [i for i in range(len(y)) if (y_pred[i] == 0 and y[i]!=y_pred[i])]
    faux_outside = [i for i in range(len(y)) if (y_pred[i] == 1 and y[i]!=y_pred[i])]
    
    # Nom de la colonne que vous souhaitez extraire
    column_names = ['Epc', 'reflist_run_id', 'refListId_actual']
    for colonne in column_names:
        # Obtenir l'indice de la colonne à partir de son nom
        column_index = pd.Index(ds.columns).get_loc(colonne)
        if colonne == 'Epc':
            false_inside_column = ds.iloc[faux_inside, column_index]
            false_outside_column = ds.iloc[faux_outside, column_index]
        elif colonne == 'reflist_run_id':
            false_inside_column1 = [elem.split('_')[0] for elem in ds.iloc[faux_inside, column_index]]
            false_outside_column1 = [elem.split('_')[0] for elem in ds.iloc[faux_outside, column_index]]
        else:
            false_inside_column2 = ds.iloc[faux_inside, column_index]
            false_outside_column2 = ds.iloc[faux_outside, column_index]
    
    details_classement = pd.DataFrame({'Tags': false_inside_column, 
                                       'Classé dans la boîte': false_inside_column1, 
                                       'Devrait être classé dans la boite': false_inside_column2})
    details_classement.drop_duplicates(inplace=True, subset='Tags')
    print(details_classement)
    # Calculer la matrice de confusion
    conf_matrix = confusion_matrix(y, y_pred)
    if not details_classement.empty:
        # Extraire les nombres des chaînes de caractères pour les EPC correctement et incorrectement classés
        correct_epcs_numbers = details_classement['Tags'].str.extract(r'(\d+)', expand=False).astype(int)
        incorrect_epcs_numbers = details_classement['Classé dans la boîte'].str.extract(r'(\d+)', expand=False).astype(int)

        # Créer une DataFrame pour epc_data pour SVM
        epc_data_svm = pd.DataFrame({
            'EPC Numbers': pd.concat([correct_epcs_numbers, incorrect_epcs_numbers]).reset_index(drop=True),
            'Classification': ['Correct'] * len(correct_epcs_numbers) + ['Incorrect'] * len(incorrect_epcs_numbers)
        })

        # Tracer le boxplot avec Seaborn pour SVM
        plt.figure(figsize=(10, 6))
        sns.boxplot(x='Classification', y='EPC Numbers', hue='Classification', data=epc_data_svm, palette='Set3', dodge=False)
        plt.title('Comparaison des EPCs correctement et incorrectement classés (SVM)')
        plt.ylabel('Nombre d\'EPCs')
        plt.grid(True)
        plt.tight_layout()
        # Définir le répertoire de sortie
        output_dir_path = os.path.join(os.getcwd(), 'wwwroot', 'images')
        os.makedirs(output_dir_path, exist_ok=True)

        # Définir le chemin du fichier à enregistrer
        file_name = 'boxplot_EPC_comparison_seaborn_KNN.png'
        plot_path = os.path.join(output_dir_path, file_name)

        # Supprimer les fichiers existants avec le même nom
        if os.path.exists(plot_path):
            os.remove(plot_path)
            print(f"Deleted existing file: {plot_path}")

        # Sauvegarder le nouveau fichier
        plt.savefig(plot_path)
        plt.close()
        print(f"Saved new file: {plot_path}")
        print('chemin :', plot_path)
        print('chemin :', plot_path)
    else:
        plot_path = None
    
    end_time = time.time()

    # Calcul du temps d'exécution
    execution_time = end_time - start_time

    return score, conf_matrix, execution_time, details_classement, plot_path

# Appel de la fonction KNN_method et récupération des résultats
'''
score, conf_matrix, execution_time, details_classement, plot_path = KNN_method(
    metric='minkowski',
    n_neighbors=5,
    n_plis=5,
    weights='uniform',
    pathfile='./data_anonymous',
    algorithm='auto',
    leaf_size=30,
    p=2,
    n_jobs=-1,
    output_dir='Img_Boxplot/New'
)
'''
