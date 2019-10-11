import { WebStorageStateStore } from 'oidc-client';

const identityAuthority = process.env.NODE_ENV === "production" ? "https://auth.nozomi.one" : 'https://localhost:6001';

export const oidcSettings = {
  authority: identityAuthority,
  client_id: 'nozomi.spa',
  redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one/oidc-callback"
    : window.location.origin + "/oidc-callback",
  response_type: 'id_token token',
  scope: 'openid profile nozomi.web.read_only',
  post_logout_redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one" : window.location.origin,
  silent_redirect_uri: process.env.NODE_ENV === "production" ? "https://nozomi.one/oidc-silent-renew"
    : window.location.origin + '/oidc-silent-renew',
  accessTokenExpiringNotificationTime: 10,
  automaticSilentRenew: true,
  filterProtocolClaims: true,
  //userStore: new WebStorageStateStore(),
  loadUserInfo: true,
  metadata: {
    issuer: identityAuthority,
    jwks_uri: identityAuthority + "/.well-known/openid-configuration/jwks",
    end_session_endpoint: identityAuthority + "/connect/endsession",
    authorization_endpoint: identityAuthority + "/connect/authorize"
  }
};
