﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = _authService.Login(userForLoginDto);
            if(!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            var result=_authService.CreateAccessToken(userToLogin.Data);
            if(result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
            
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto userForRegisterDto)
        {
            var userExits = _authService.UserExists(userForRegisterDto.Email);
            if(!userExits.Success)
            {
                return BadRequest(userExits.Message);
            }
            var registerResult = _authService.Register(userForRegisterDto, userForRegisterDto.Password);
            var result = _authService.CreateAccessToken(registerResult.Data);
            if(result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.Message);
         

        }


    }
}