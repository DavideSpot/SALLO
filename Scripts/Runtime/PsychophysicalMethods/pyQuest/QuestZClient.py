""" QuestZClient

This script lets developers use the python implementation of the Quest psychophysical procedure in Unity.
It does so via a Python for Unity out-of-process API and the implementation of the Quest algorithm contained in the VisionEgg Python package.

"""

# Best practice: Import unity_client first, because it sets some settings
# appropriately. That allows running from the command-line.
import unity_python.client.unity_client as unity_client
import unity_python.client.evalexec_client as evalexec_client

import inspect
import logging
import os
import sys
import time
import threading
import traceback
import socket

import Quest
import numpy as np
from numpy import linalg as la
from typing import NamedTuple


client_name = "Quest Z Client"
# hard-coded name of the Python client. If edited, also the hard-coded string 'ClientName' in the c# class pyQuest must be edited
try_to_connect = True

class QuestClient(unity_client.UnityClientService):
    """
    The class exposing the Quest features to Unity.
    The Quest implementation used here is forced to keep its value betwee 0 and 1. Further scaling is performed in Unity.
    See Quest.py for the complete documentation about the Quest class

    ...

    Attributes
    ----------

    tGuess : float
        The prior threshold estimate.
    tGuessSD : float
        the standard deviation assigned to tGuess.
    pThreshold : float
        the threshold criterion expressed as probability of response==1. See Quest.py for further details.
    beta : float
        controls the steepness of the (Weibull) psychometric function. Typically 3.5, so as to have symmetric PDF.
    gamma : float
        the fraction of trials that will generate response 1 when intensity==-inf.
    delta : float
        the fraction of trials on which the observer presses blindly. Typically 0.01.
    noise : float
        the standard deviation of the gaussian noise which can be added to the Quest estimate
    
    beta, delta, and gamma are the parameters of a Weibull
    psychometric function.
    
    Quest range fixed at 1 

    Methods (the methods with the 'exposed' prexif are callable from Unity)
    -------
    exposed_initQuest(Grain = 0.01)
        Instantiate a Quest object with the desired grain

    _clearQuest()
        Destroy the current QuestObject instance and clear the stored stimuli and responses
    
    exposed_restart_quest()
        Destroy the current questObject instance and its related history, and instantiate a new one

    exposed_gausnoise()
        Get a sample from a gaussian noise distribution with 'self.noise' standard deviation 

    exposed_query_quest(addNoise, append, verbose, randomFlip)
        Get the most informative value for the stimulus according to the QuestObject instance

    exposed_update_quest(response, zstim_real, append, verbose)
        Update the Quest algorithm's estimate according the the participant's response
    

    exposed_client_name()
        Get the QuestZClient clientname attribute

    """
    tGuess = 0.5
    tGuessSd = 1
    pThreshold = 0.5
    beta = 3.5
    gamma = 0.01
    delta = 0.01
    noise = .05

    def exposed_initQuest(self,Grain = 0.01):
        """Instantiate a Quest object with the desired grain

        Parameters
        ----------
        Grain : float
            the quantization of the internal table. E.g. 0.01.
        """
        self.stimuli = []
        self.responses = []
        self.quest = Quest.QuestObject(tGuess=QuestClient.tGuess, tGuessSd=QuestClient.tGuessSd, pThreshold=QuestClient.pThreshold, beta=QuestClient.beta, gamma=QuestClient.gamma, # no numero predefinito trial. Da definire in main ###
                                    delta=QuestClient.delta, grain=Grain, range=1)
        print("initialized with grain: {g}".format(g = Grain))


    def _clearQuest(self):
        """
        Destroy the current QuestObject instance and clear the stored stimuli and responses
        """
        del self.stimuli
        del self.responses
        del self.quest
        print("cleared")

    def exposed_restart_quest(self):
        """
        Destroy the current questObject instance and its related history, and instantiate a new one
        """
        Grain = self.quest.grain
        self._clearQuest()
        self.exposed_initQuest(Grain)
    
    def exposed_gausnoise(self):
        """
        Get a sample from a gaussian noise distribution with 'self.noise' standard deviation
        Returns
        -------
        sample : float
            the noise sample
        """
        sample = np.random.randn()*self.noise
        print("Noise is {s}".format(s= sample))
        return sample
    
    def exposed_query_quest(self, addNoise = False, append = True, verbose = True, randomFlip = True):
        """
        Get the most informative value for the stimulus according to the QuestObject instance

        Parameters
        ----------
        addNoise : bool
            inject some gaussian random noise in the Quest estimate?
        append : bool, optional
            keep track of the query?
        verbose : bool
            output the queried value to console?
        randomFlip : bool
            Take the value symmetrical to the Quest estimate with respect to 0.5?
            Necessary to balance left and right trials (the quest is designed for mono directional intensity assessment (Quest converges at 0),
            while left-right response patterns require bidirectional intensity assessment. Solution: random flipping to make the Quest converge at 0.5.            
        Returns
        -------
        zstim_best
            The (preprocessed?) Quest estimate
        """
        zstim_best = self.quest.quantile()
        if verbose:
            print("best z-stim is {s}".format(s= zstim_best))
        if randomFlip:
            zstim_best = 0.5 + (zstim_best-0.5)*[-1,1][np.random.randint(0,2)]
        if addNoise:
            zstim_best += np.random.randn()*self.noise
            if verbose:
                print("quasi-best z-stim is {s}".format(s= zstim_best))
        if append:
            self.stimuli.append(zstim_best)    
        return zstim_best

    def exposed_update_quest(self, response, zstim_real = None, append = True, verbose = True):
        """
        Update the Quest algorithm's estimate according the the participant's response

        Parameters
        ----------
        response : int
            The participant's response: 1 if hit or 0 if miss
        zstim_real : float, optional
            The value used as stimulus. If empty, the last raw Quest estimate is used
        append : bool, optional
            keep track of the response?
        verbose : bool
            output the stimulus-response pair to console?
        """
        if zstim_real is None:
            self.quest.update(self.stimuli[-1], response)
            if verbose:
                print("test at {t}, answer is {r}".format(t=self.stimuli[-1],r=response))
        else:
            self.quest.update(zstim_real, response)
            if verbose:
                print("test at {t}, answer is {r}".format(t=zstim_real,r=response))
        if append:
            self.responses.append(response)

    def exposed_client_name(self):
        return client_name


if __name__ == '__main__':
    np.random.RandomState()
    time.sleep(0.5)
    # This is the loop which maintains the connection to Unity.
    # It handles reconnection, and whether it is needed (try_to_connect may be false).
    while try_to_connect:
        
        time.sleep(0.5)

        try:
            # Here starts a new thread connecting to Unity.
            # It listens to incoming messages, and uses the defined service.
            q = QuestClient()
            c = unity_client.connect(q)
            try_to_connect = False
        except socket.error:
            print("failed to connect; try again later")
            time.sleep(0.5)
            continue
            

        print("connected")
        try:
            c.thread.join() # Wait for KeyboardInterrupt (^c or server quit)
        except KeyboardInterrupt:
            c.close()
            c.thread.join() # Wait for the thread to notice the close()
        print("disconnected")   

