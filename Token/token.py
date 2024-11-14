import hmac
import hashlib
import base64

class TokenHelper:
    # Pre-shared key
    pre_shared_key = "Jo7ygf789.Token"

    @staticmethod
    def generate_token():
        # Create a new HMAC object using SHA256 and the pre-shared key
        hmac_obj = hmac.new(
            key=TokenHelper.pre_shared_key.encode('utf-8'),
            msg=TokenHelper.pre_shared_key.encode('utf-8'),
            digestmod=hashlib.sha256
        )
        # Compute the HMAC hash
        hash_value = hmac_obj.digest()
        # Return the Base64 encoded hash
        return base64.b64encode(hash_value).decode('utf-8')

    @staticmethod
    def validate_token(token):
        # Generate the expected token
        expected_token = TokenHelper.generate_token()
        # Validate the provided token against the expected token
        return token == expected_token

# Example usage
if __name__ == "__main__":
    # Generate token
    token = TokenHelper.generate_token()
    print("Generated Token:", token)

    # Validate the token
    is_valid = TokenHelper.validate_token(token)
    print("Is Valid Token:", is_valid)