import Oidc from "oidc-client";

export const oidcSettings = {
  authority: process.env.NODE_ENV === "production" ? "https://auth.nozomi.one" : 'https://localhost:6001',
  client_id: 'nozomi.spa',
  redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one/oidc-callback" : 'https://localhost:5001/oidc-callback',
  response_type: 'id_token token',
  scope: 'openid profile nozomi.web.read_only',
  post_logout_redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one" : 'https://localhost:5001/',
  userStore: new Oidc.WebStorageStateStore({ store: window.localStorage }),
  loadUserInfo: true
};
