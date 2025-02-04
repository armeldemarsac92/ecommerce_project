'use client';

import {useContext, createContext} from "react";

export interface IAuthData {
  current_email: string;
}

interface IAuthContext {
  authData: IAuthData | undefined;
  contextLoading: boolean;
  updateContextLoading: (isLoading: boolean) => void;
  updateAuthData: (data: IAuthData) => void;
}

export const AuthContext = createContext<IAuthContext | undefined>(undefined);

export const useAuthContext = () => {
  const context = useContext(AuthContext);

  if (!context) throw new Error('useAuthContext must be used within AuthProvider');

  return context;
};