﻿using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace TestForMe
{

    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float speed = 55;

        [SerializeField] private int jumpForce = 5;

        [SerializeField] private TextMeshProUGUI countText;
        [SerializeField] private GameObject winTextObject;

        private Rigidbody _rigidBody;

        private int _countToWin = 0;

        private bool _isGrounded;

        private int _extraJumps;
        private int _extraJumpValue = 1;

        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();

            _playerInput.Player.Jump.performed += context => Jump();
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();

            SetCountText();
        }

        private void Update()
        {
            Vector2 moveDirection = _playerInput.Player.Move.ReadValue<Vector2>();

            Move(moveDirection);

            GroundCheker();
        }

        private void Move(Vector2 direction)
        {
            Vector3 dirVector3 = new Vector3(direction.x, 0f, direction.y);

            _rigidBody.AddForce(dirVector3 * speed);
        }

        private void GroundCheker()
        {
            int checker = 0;

            foreach (Collider collider in Physics.OverlapSphere(transform.position, 1f))
            {
                if (collider.CompareTag("Ground"))
                {
                    checker++;
                }
            }

            if (checker > 0)
            {
                _isGrounded = true;

                _extraJumps = _extraJumpValue;
            }
            else
            {
                _isGrounded = false;
            }
        }

        private void Jump()
        {
            if (_isGrounded || _extraJumps > 0)
            {
                _rigidBody.AddForce(Vector3.up * jumpForce);

                _extraJumps--;
            }
        }

        private void SetCountText()
        {
            countText.text = $"Count: {_countToWin.ToString()}";

            if (_countToWin >= 12)
            {
                winTextObject.SetActive(true);

                StartCoroutine(RestartGame(3));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("PickUp"))
            {
                other.gameObject.SetActive(false);

                _countToWin = _countToWin + 1;

                SetCountText();
            }
            if (other.gameObject.CompareTag("DeadZone"))
            {
                StartCoroutine(RestartGame(1));
            }
        }

        private IEnumerator RestartGame(int delayTime)
        {
            yield return new WaitForSeconds(delayTime);

            SceneManager.LoadScene(0);
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }
        private void OnDisable()
        {
            _playerInput.Disable();
        }
    }

}