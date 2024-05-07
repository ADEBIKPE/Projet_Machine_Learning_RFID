def RandomForest_method(n_arbres, profondeur, n_plis, n_minimum_split, pathfile, criterion, min_samples_leaf,
                         min_weight_fraction_leaf, max_features, max_leaf_nodes, min_impurity_decrease,
                         bootstrap, oob_score, n_jobs, random_state, verbose, warm_start, class_weight, ccp_alpha, max_samples):
    # Charger les données
    import time
    from preprocessing import preprocessing
    from sklearn.model_selection import cross_val_score, cross_val_predict
    from sklearn.ensemble import RandomForestClassifier
    from selectfeatures import Xcols_func
    import pandas as pd
    from sklearn.metrics import confusion_matrix
    
    start_time = time.time()
    
    X = Xcols_func('rssi & rc only', preprocessing(pathfile).columns)
    ds = preprocessing(pathfile)
    x = ds.loc[:, X]
    y = ds['in_or_out']
    
    # Créer le modèle avec tous les hyperparamètres spécifiés
    clf = RandomForestClassifier(n_estimators=n_arbres, max_depth=profondeur, min_samples_split=n_minimum_split, criterion=criterion,
                                  min_samples_leaf=min_samples_leaf, min_weight_fraction_leaf=min_weight_fraction_leaf, 
                                  max_features=max_features, max_leaf_nodes=max_leaf_nodes, min_impurity_decrease=min_impurity_decrease,
                                 bootstrap=bootstrap, oob_score=oob_score, n_jobs=n_jobs, random_state=random_state, 
                                    verbose=verbose, warm_start=warm_start, class_weight=class_weight, ccp_alpha=ccp_alpha, max_samples=max_samples)
    
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
    
    # Calcul de la matrice de confusion
    cm = confusion_matrix(y, y_pred)
    
    end_time = time.time()

    # Calcul du temps d'exécution
    execution_time = end_time - start_time

    return score, cm, execution_time, details_classement
