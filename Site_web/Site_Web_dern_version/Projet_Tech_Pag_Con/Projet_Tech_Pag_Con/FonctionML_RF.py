def RandomForest_method(n_arbres, profondeur, n_plis, n_minimum_split, pathfile, criterion, min_samples_leaf,
                         min_weight_fraction_leaf, max_features, max_leaf_nodes, min_impurity_decrease,
                         bootstrap, oob_score, n_jobs, random_state, verbose, warm_start, class_weight, ccp_alpha, max_samples,
                         output_dir):
    # Charger les données
    from preprocessing import preprocessing
    from sklearn.model_selection import cross_val_score, cross_val_predict
    from sklearn.ensemble import RandomForestClassifier
    from selectfeatures import Xcols_func
    from sklearn.metrics import confusion_matrix
    import pandas as pd
    import os
    import time
    import matplotlib
    matplotlib.use('Agg')  # Utiliser la backend Agg de Matplotlib pour éviter les problèmes de thread
    import matplotlib.pyplot as plt
    import seaborn as sns

    start_time = time.time()

    X = Xcols_func('rssi & rc only', preprocessing(pathfile).columns)
    ds = preprocessing(pathfile)
    x = ds.loc[:, X]
    y = ds['in_or_out']

    # Créer le modèle avec tous les hyperparamètres spécifiés
    clf = RandomForestClassifier(n_estimators=n_arbres, max_depth=profondeur, min_samples_split=n_minimum_split,
                                 criterion=criterion, min_samples_leaf=min_samples_leaf,
                                 min_weight_fraction_leaf=min_weight_fraction_leaf,
                                 max_features=max_features, max_leaf_nodes=max_leaf_nodes,
                                 min_impurity_decrease=min_impurity_decrease,
                                 bootstrap=bootstrap, oob_score=oob_score, n_jobs=n_jobs,
                                 random_state=random_state, verbose=verbose, warm_start=warm_start,
                                 class_weight=class_weight, ccp_alpha=ccp_alpha, max_samples=max_samples)

    # Effectuer une validation croisée
    scores = cross_val_score(clf, x, y, cv=n_plis)

    # Afficher les scores de validation croisée
    print("Scores de validation croisée:", scores)
    print("Score moyen de validation croisée:", scores.mean())
    score = scores.mean()

    # Entraîner le modèle sur toutes les données
    y_pred = cross_val_predict(clf, x, y, cv=n_plis)
    faux_inside = [i for i in range(len(y)) if y_pred[i] == 'inside' and y[i] == 'outside']
    faux_outside = [i for i in range(len(y)) if y_pred[i] == 'outside' and y[i] == 'inside']

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
    details_classement.drop_duplicates(inplace=True)

    if details_classement.empty:
        print("Aucun mauvais classement trouvé.")
    else:
        print(details_classement)

    # Matrice de confusion
    cm = confusion_matrix(y, y_pred)
    end_time = time.time()
    execution_time = end_time - start_time

    if not details_classement.empty:
        correct_epcs_numbers = details_classement['Tags'].str.extract(r'(\d+)', expand=False).astype(int)
        incorrect_epcs_numbers = details_classement['Classé dans la boîte'].str.extract(r'(\d+)', expand=False).astype(int)

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

        # Définir le répertoire de sortie
        output_dir_path = os.path.join(os.getcwd(), 'wwwroot', 'images')
        os.makedirs(output_dir_path, exist_ok=True)

        # Définir le chemin du fichier à enregistrer
        file_name = 'boxplot_EPC_comparison_seaborn_RF.png'
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

    return score, cm, execution_time, details_classement, plot_path
'''
# Appel de la fonction avec des paramètres ajustés
score, cm, execution_time, details_classement, plot_path = RandomForest_method(
    n_arbres=100,
    profondeur=10,  # Augmentation de la profondeur pour une meilleure modélisation
    n_plis=5,
    n_minimum_split=2,
    pathfile='Site_web/Site_Web_dern_version/Projet_Tech_Pag_Con/Projet_Tech_Pag_Con/data_anonymous',
    criterion='gini',
    min_samples_leaf=1,
    min_weight_fraction_leaf=0.1,  # Suppression de la fraction minimale pour éviter la restriction
    max_features='sqrt',
    max_leaf_nodes=None,  # Suppression de la limite de noeuds pour permettre plus de flexibilité
    min_impurity_decrease=0.1,  # Suppression de la limite d'impureté pour permettre plus de flexibilité
    bootstrap=True,
    oob_score=False,
    n_jobs=-1,
    random_state=44,
    verbose=3,
    warm_start=False,
    class_weight='balanced',
    ccp_alpha=0.1,  # Suppression de la complexité du coût-pruning pour plus de flexibilité
    max_samples=None,  # Suppression de l'échantillonnage maximum pour utiliser tout l'ensemble de données
    output_dir='Img_Boxplot/New'
)
'''