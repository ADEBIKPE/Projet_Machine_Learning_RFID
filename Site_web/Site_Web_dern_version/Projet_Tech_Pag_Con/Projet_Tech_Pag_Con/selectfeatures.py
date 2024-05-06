def Xcols_func(features, Xcols_all):
    import pandas as pd
    import numpy as np
    Features=pd.DataFrame(\
    [\
    ['all', True, True, False, True, True, True ],\
    ['rssi & rc only', True, True, False, False, False, False ], \
    ['rssi & rc_mid', True, True, True, False, False, False ], \
    ['rssi only', True, False, True, False, False, False ], \
    ['rc only', False, True, False, False, False, False ], \
    ], columns=['features', 'rssi', 'rc', 'rc_mid_only', 'Epcs_window', 'reads_window', 'window_width'])

    Features_temp = Features[Features['features'] == features]

    X = []
    rssi = Features_temp['rssi'].values[0]
    rc = Features_temp['rc'].values[0]
    rc_mid_only = Features_temp['rc_mid_only'].values[0]
    Epcs_window = Features_temp['Epcs_window'].values[0]
    reads_window = Features_temp['reads_window'].values[0]
    window_width = Features_temp['window_width'].values[0]

    X_rssi = [x for x in Xcols_all if rssi and 'rssi' in x.split('_')]
    X_rc = [x for x in Xcols_all if rc and 'rc' in x.split('_')]

    X = X_rssi + X_rc

    if Epcs_window:
        X.append('Epcs_window')

    if reads_window:
        X.append('reads_window')

    if window_width:
        X.append('window_width')

    return X